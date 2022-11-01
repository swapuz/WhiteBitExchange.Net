using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net.Clients
{
    public abstract class WhiteBitSocketCommonClient : SocketApiClient, IWhiteBitSocketClientCommonStream
    {
        protected Log _log;
        protected WhiteBitSocketClient _whiteBitSocketClient;
        public WhiteBitSocketCommonClient(Log log, WhiteBitSocketClient whiteBitSocketClient, WhiteBitSocketClientOptions options) :
            base(options, options.SpotStreamsOptions)
        {
            this._log = log;
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
    return await SubscribeToCurrentCandles(
        openCandles => 
        {
            foreach (var candle in openCandles)
            {
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

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new WhiteBitAuthenticationProvider(credentials);

        internal async Task<CallResult<UpdateSubscription>> SubscribeInternal<TRequest, TUpdate>(
            WhiteBitSocketRequest<TRequest> request,
            bool authenticate,
            Action<TUpdate?> onData,
            CancellationToken ct)
        {
            return await _whiteBitSocketClient.SubscribeInternal<TRequest, WhiteBiteSocketUpdateEvent<TUpdate>>(
                this,
                BaseAddress,
                request,
                authenticate,
                (dataWithServiceInfo) => onData(dataWithServiceInfo.Data.Data),
                ct);
        }
    }
}