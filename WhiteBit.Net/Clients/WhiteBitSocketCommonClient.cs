using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;
using CryptoExchange.Net;

namespace WhiteBit.Net.Clients
{
    public abstract class WhiteBitSocketCommonClient : SocketApiClient, IWhiteBitSocketClientCommonStream
    {
        private static string? WebsocketToken;
        private static SemaphoreSlim WebSocketTokenSemaphore = new SemaphoreSlim(1, 1);
        protected WhiteBitSocketClient _whiteBitSocketClient;
        private readonly WhiteBitSocketClientOptions _options;

        protected WhiteBitSocketCommonClient(ILogger log, WhiteBitSocketClient whiteBitSocketClient, WhiteBitSocketClientOptions options) :
            base(log,"",options.SocketExchangeOptions, options.SocketApiOptions)
        {
            _options = options; 
            this._whiteBitSocketClient = whiteBitSocketClient;
        }

        ///<inheritdoc/>>
        public async Task<CallResult<UpdateSubscription>> SubscribeToActiveOrders(Action<OrderSocketUpdate?> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.ActiveOrdersSubscribe, symbols.ToUpper()),
                true,
                dataHandler,
                ct
            );
        }
        /// <inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToExecutedOrders(Action<IEnumerable<WhiteBitOrder>?> dataHandler, CancellationToken ct = default, WhiteBitOrderType filter = WhiteBitOrderType.Any, params string[] symbols)
        {
            filter = (int)filter > 2 ? WhiteBitOrderType.Any : filter;
            return await SubscribeInternal(
                new WhiteBitSocketRequest<object>(SocketOutgoingMethod.ExecutedOrdersSubscribe, new object[] {symbols.ToUpper(), filter}),
                true,
                dataHandler,
                ct
            );
        }
        /// <inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToUserTrades(Action<WhiteBitUserTrade> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal<IEnumerable<string>, WhiteBitUserTradeAsArray>(
                new WhiteBitSocketRequest<IEnumerable<string>>(SocketOutgoingMethod.UserTradesSubscribe, new List<IEnumerable<string>>() {symbols.ToUpper()}),
                true,
                tradeAsArray => dataHandler(tradeAsArray!.Convert()!),
                ct
            );
        }
        /// <inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToCurrentCandles(Action<IEnumerable<WhiteBitCandle>> dataHandler, string symbol, int intervalInSeconds, CancellationToken ct = default)
        {
            return await SubscribeInternal(
                new WhiteBitSocketRequest<object>(SocketOutgoingMethod.CandlesSubscribe, new List<object>() { symbol, intervalInSeconds }),
                false,
                dataHandler!,
                ct
            );
        }
        /// <inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToRecentlyClosedCandles(Action<WhiteBitCandle> dataHandler, string symbol, int intervalInSeconds, CancellationToken ct = default)
        {
            WhiteBitCandle? latestCandle = null;
            var loker = new object();
            return await SubscribeToCurrentCandles(
                openCandles => 
                {
                    lock(loker)
                        foreach (var candle in openCandles)
                        {
                            if (latestCandle?.Timestamp > candle.Timestamp)
                                continue; // old candle may be sent twice by whitebit, skip second handling
                            if (latestCandle?.Timestamp < candle.Timestamp )
                            {
                                dataHandler(latestCandle);
                            }
                            latestCandle = candle;
                        }
                },
                symbol,
                intervalInSeconds,
                ct
            );
        }
        /// <inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToLastPrice(Action<LastPrice> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.LastpriceSubscribe, symbols),
                false,
                dataHandler!,
                ct
            );
        }
        /// <inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToUtcDayStartTicker(Action<WhiteBitTicker> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal<string, WhiteBitTickerAsArray>(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.TickerUtcDaySubscribe, symbols),
                false,
                rawTicker => {
                    var ticker = rawTicker!.Body;
                    ticker!.Symbol = rawTicker.Symbol!;
                    dataHandler(ticker);
                },
                ct
            );
        }
        /// <inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeTo24HAgoTicker(Action<WhiteBitCustomPeriodTicker> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal<string, WhiteBitCustomPeriodTickerAsArray>(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.Ticker24HSubscribe, symbols),
                false,
                rawTicker =>
                {
                    var ticker = rawTicker!.Body;
                    ticker!.Symbol = rawTicker.Symbol!;
                    dataHandler(ticker);
                },
                ct
            );
        }
        /// <inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToPublicTrades(
            Action<KeyValuePair<string, IEnumerable<WhiteBitPublicTrade>>> dataHandler,
            CancellationToken ct = default,
            params string[] symbols)
        {
            return await SubscribeInternal<string, publicTradesAsArray>(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.PublicTradesSubscribe, symbols),
                false,
                rawTrades =>
                {
                    rawTrades!.Body.ToList().ForEach(trade => 
                        { 
                            trade.Symbol = rawTrades.Symbol;
                            trade.QuoteVolume = trade.Price * trade.BaseVolume;
                        });

                    dataHandler(new KeyValuePair<string, IEnumerable<WhiteBitPublicTrade>>(rawTrades.Symbol, rawTrades.Body));
                },
                ct
            );
        }
        public async Task<CallResult<UpdateSubscription>> SubscribeToOrderBook(
            Action<WhiteBitSocketOrderBook> onUpdate,
            string symbol,
            int maxEntriesAmount = 100,
            OrderBookSocketAggregationLevel aggregationLevel = OrderBookSocketAggregationLevel.NoAggregation,
            CancellationToken ct = default)
        {
            string aggregationParam = aggregationLevel switch
            {
                OrderBookSocketAggregationLevel.NoAggregation => "0",
                _ => Math.Pow(10, (double)aggregationLevel).ToString(CultureInfo.InvariantCulture)
            };
            return await SubscribeInternal(
                new WhiteBitSocketRequest<object>(SocketOutgoingMethod.OrderBookSubscribe, new List<object>
                {
                    symbol,
                    maxEntriesAmount,
                    aggregationParam,
                    true
                }),
                false,
                onUpdate!,
                ct
            );
        }
        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new WhiteBitAuthenticationProvider(credentials);

        internal async Task<CallResult<UpdateSubscription>> SubscribeInternal<TRequest, TUpdate>(
            WhiteBitSocketRequest<TRequest> request,
            bool authenticate,
            Action<TUpdate?> onData,
            CancellationToken ct)
        {
            return await SubscribeInternal<TRequest, WhiteBiteSocketUpdateEvent<TUpdate>>(
                BaseAddress,
                request,
                authenticate,
                (dataWithServiceInfo) => onData(dataWithServiceInfo.Data.Data),
                ct).ConfigureAwait(false);
        }


        protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection socketConnection)
        {
            // should I check this condition?
            if (socketConnection.ApiClient.AuthenticationProvider is null)
                return new CallResult<bool>(false);
            bool isSuccess = false;
            ServerError? serverError = null;
            await socketConnection.SendAndWaitAsync(
                    new AuthorizeSocketRequest(
                            ExchangeHelpers.NextId(),
                            (await GetWebsocketToken(socketConnection.ApiClient.AuthenticationProvider))!),
                            _options.TimeOut, null,_options.Weight,
                            data =>
                            {
                                AuthorizeSocketResponse? result = null;
                                try
                                {
                                    result = data.ToObject<AuthorizeSocketResponse>();
                                }
                                catch (Exception e)
                                {
                                    serverError = new ServerError($"Error while parse into AuthorizeSocketResponse: {e.Message}");
                                    return false;
                                }

                                if (data.ToObject<AuthorizeSocketResponse>()?.Result?.Status == SubscriptionStatus.Success)
                                {
                                    isSuccess = true;
                                }
                                else
                                {
                                    serverError = new ServerError($"Auth request was not successful: {result!.Error?.ToString()}");
                                }
                                return true;
                            });
            return isSuccess ? new CallResult<bool>(true) : new CallResult<bool>(serverError!);
        }

        private static async Task<string?> GetWebsocketToken(AuthenticationProvider authProvider)
        {
            try
            {
                await WebSocketTokenSemaphore.WaitAsync();
                if (WebsocketToken is null && authProvider is WhiteBitAuthenticationProvider authenticationProvider)
                {
                    using (var client = new WhiteBitRestClient(
                        new WhiteBitRestClientOptions()
                        {
                            ApiCredentials = authenticationProvider.Credentials
                        }, null)
                    )
                    {
                        WebsocketToken = await ((WhiteBitApiClientV4)client.ApiClient).GetWebsocketToken();
                    }
                }
                return WebsocketToken;
            }
            finally
            {
                WebSocketTokenSemaphore.Release();
            }
        }

        protected override bool HandleQueryResponse<T>(SocketConnection socketConnection, object request, JToken data, [NotNullWhen(true)] out CallResult<T>? callResult)
        {
            if (!IsResponseMatchesToRequest(request, data))
            {
                callResult = null;
                return false;
            }
            var response = data.ToObject<WhiteBitSocketResponse<T>>();
            callResult = (response is null || response.Result == null) ?
                                    new CallResult<T>(new ServerError((int)(response?.Error?.Error ?? 0), response?.Error?.ToString() ?? "Empty data came"))
                                    : new CallResult<T>(response.Result);
            return true;
        }

        protected override bool HandleSubscriptionResponse(SocketConnection socketConnection, SocketSubscription subscription, object request, JToken data, out CallResult<object>? callResult)
        {
            if (!HandleQueryResponse(socketConnection, request, data, out callResult))
            {
                return false;
            }
            if (callResult.Success)
                _logger.LogInformation($"Socket {socketConnection.SocketId} Subscription completed");
            return true;
        }

        private bool IsResponseMatchesToRequest(object request, JToken data)
        {
            if (data.Type != JTokenType.Object)
                return false;

            var respId = (int?)data["id"];
            if (respId == null || respId != ((IWhiteBitSocketDataMethod<SocketOutgoingMethod>)request).Id)
                return false;

            return true;
        }

        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            var methodOut = ((IWhiteBitSocketDataMethod<SocketOutgoingMethod>)request).Method;
            return methodOut.DoesMethodMatch(message["method"]?.ToObject<SocketIncomeMethod>()) == true;
        }

        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            return true;
        }

        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscriptionToUnsub)
        {
            var unSubReq = ((WhiteBitSocketRequest<object>)subscriptionToUnsub.Request!).Method.GetCorrespondingUnsubscribeMethod();
            if (unSubReq is null)
                return false;
            var request = new UnsubscribeRequest(ExchangeHelpers.NextId(), unSubReq.Value);
            var result = await QueryAndWaitAsync<SocketStatusResult>(connection, request, _options.Weight);
            return result.Data?.Status == SubscriptionStatus.Success;
        }

        internal async Task<CallResult<UpdateSubscription>> SubscribeInternal<TRequest, TUpdate>
            (
            string url,
            WhiteBitSocketRequest<TRequest> request,
            bool authenticate,
            Action<DataEvent<TUpdate>> onData,
            CancellationToken ct)
        {
            request.Id = ExchangeHelpers.NextId();
            return await SubscribeAsync(url, request, null, authenticate, onData, ct);
        }
    }
}