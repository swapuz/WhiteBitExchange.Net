using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Requests;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net.Clients
{
    public class WhiteBitApiClientV4 : WhiteBitApiClient, IWhiteBitApiClientV4
    {
        #region endpoints
        private const string TickerUrl = "public/ticker";
        private const string AssetsUrl = "public/assets";
        private const string OrderBookUrl = "public/orderbook/{market}";
        private const string PublicTradesUrl = "public/trades/{market}";
        private const string FeeUrl = "public/fee";
        private const string ServerTimeUrl = "public/time";
        private const string ServerPingUrl = "public/ping";
        private const string FuturesListUrl = "public/futures";
        private const string CollateralMarketsUrl = "public/collateral/markets";
        private const string BalanceUrl = "trade-account/balance";//
        private const string PlaceLimitOrderUrl = "order/new";
        private const string PlaceStopLimitOrderUrl = "order/stop_limit";
        private const string PlaceMarketOrderUrl = "order/market";
        private const string PlaceStopMarketOrderUrl = "order/stop_market";
        private const string PlaceStockMarketOrderUrl = "order/stock_market";
        private const string CancelOrderUrl = "order/cancel";
        private const string ActiveOrdersUrl = "orders";
        private const string FilledOrdersUrl = "trade-account/order/history";
        private const string OrderTradesUrl = "trade-account/order";
        private const string OwnTradesUrl = "trade-account/executed-history";
        private const string WebsocketTokenUrl = "profile/websocket_token";

        private const string DepositeAddress = "main-account/address";
        private const string CreateDepositeAddress = "main-account/create-new-address";
        private const string GetDepositeWsitrawalHistory = "main-account/history";
        private const string WithdrawRequst = "main-account/withdraw";
        private const string MainBalanceRequst = "main-account/balance";
        private const string WithdrawPayRequst = "main-account/withdraw-pay";
        private const string GetTransferRequest = "main-account/transfer";
        private const string AccountBalanceSummary = "collateral-account/balance-summary";
        //Collateral Account Balance Summary
        //[POST] /api/v4/

        #endregion
        internal WhiteBitApiClientV4(string name, WhiteBitRestClientOptions options, RestApiClientOptions apiOptions, ILogger log, WhiteBitRestClient client) : base(name, options, apiOptions, log, client)
        {
        }

        protected override string ApiVersion => "4";
        internal static TimeSyncState TimeSyncState = new TimeSyncState("Spot Api");


        public event Action<OrderId>? OnOrderPlaced;
        public event Action<OrderId>? OnOrderCanceled;

        /// <summary>
        /// This V4 endpoint can be used to retrieve the websocket token for user.
        /// </summary>
        internal async Task<string?> GetWebsocketToken( CancellationToken ct = default)
        {
            var result =  await SendRequestAsync<JToken>(WebsocketTokenUrl, ct);
            if (!result)
            {
                //_log.Write(LogLevel.Error, $"Failed to get websocket token:\n {result.Error?.Message}");
                return null;
            }
            return result.Data["websocket_token"]!.ToString();
        }

        #region IWhiteBitApiClientV4 Methods
        ///<inheritdoc/>
        public async Task<WebCallResult<WhiteBitTradingBalance?>> GetBalanceAsync(string currency, CancellationToken ct = default)
        {
            currency = currency.ToUpper();
            var result =  await SendRequestAsync<WhiteBitRawTradingBalance>(BalanceUrl, ct, new Dictionary<string, object>{{"ticker", currency}});
            return result.As(result.Data.Convert(new WhiteBitTradingBalance {Currency = currency}));
        }
        public async Task<WebCallResult<WhiteBitMainBalance?>> GetMainBalanceAsync(string currency, CancellationToken ct = default)
        {
            currency = currency.ToUpper();
            var result = await SendRequestAsync<WhiteBitRawMainBalance>(MainBalanceRequst, ct, new Dictionary<string, object> { { "ticker", currency } });
            return result.As(result.Data.Convert(new WhiteBitMainBalance { Currency = currency }));
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitTradingBalance>?>> GetBalancesAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<Dictionary<string, WhiteBitRawTradingBalance>>(BalanceUrl, ct);
            return result.As(result.Data?.Select(b => b.Value.Convert(new WhiteBitTradingBalance { Currency = b.Key })!));
            
        }

        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<AccountBalanceSummaryItemResponse>>> GetAccountBalanceSummaryAsync(
            string sumbols, CancellationToken ct = default)
        {
            
            var result =
                await SendRequestAsync<IEnumerable<AccountBalanceSummaryItemResponse>>(AccountBalanceSummary, ct,
                    new Dictionary<string, object> { { "ticker", sumbols } });
            return result.As(result.Data);
        }

        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitRestTicker>?>> GetTickersAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<Dictionary<string, WhiteBitRawRestTicker>>(TickerUrl, ct);
            return result.As(result.Data?.Select(b => b.Value.Convert(new WhiteBitRestTicker { Symbol = b.Key })!));
        }

        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitAsset>?>> GetAssetsAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<Dictionary<string, WhiteBitRawAsset>>(AssetsUrl, ct);
            return result.As(result.Data?.Select(b => b.Value.Convert(new WhiteBitAsset {Currency = b.Key} )!));
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<WhiteBitOrderBook>> GetOrderBookAsync(string symbol, int? depthLimit = null, int? aggregationLevel = null, CancellationToken ct = default)
        {
            var param = new Dictionary<string, object>();
            param.AddOptionalParameter("limit", depthLimit);
            param.AddOptionalParameter("level", aggregationLevel);
            return await SendRequestAsync<WhiteBitOrderBook>(FillPathParameter(OrderBookUrl, symbol), ct, param);
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitPublicTrade>?>> GetPublicTradesAsync(string symbol, WhiteBitOrderSide? side = null, CancellationToken ct = default)
        {
            var param = new Dictionary<string, object>();
            param.AddOptionalParameter("type", side?.ToString().ToLower());
            var result = await SendRequestAsync<List<WhiteBitPublicTrade>>(FillPathParameter(PublicTradesUrl, symbol), ct, param);
            return result.As(result.Data?.Select(tr => tr.Convert<WhiteBitPublicTrade>(new() { Symbol = symbol })!));
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<List<WhiteBitFutures>?>> GetFuturesAsync(CancellationToken ct = default)
        {
            var result =  await SendRequestAsync<BaseResponse<List<WhiteBitFutures>>>(FuturesListUrl, ct);
            return result.As(result.Data?.Result);
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitAssetFee>?>> GetFeeAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync< Dictionary <string, WhiteBitRawAssetFee >> (FeeUrl, ct);
            var res =  result.As(result.Data?.Select(b => b.Value.Convert(new WhiteBitAssetFee
            {
                //Currency = b.Key,
                Network = Regex.Replace(b.Key, @"\w+ (\(\w+\))","$1", RegexOptions.IgnoreCase).Replace("(","").Replace(")","")
            })!));
            return res;
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<List<string>>> GetCollateralMarketsAsync(CancellationToken ct = default)
        {
            return await SendRequestAsync<List<string>>(CollateralMarketsUrl, ct);
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<WhiteBitOrder>> PlaceOrderAsync(WhiteBitPlaceOrderRequest parameters ,CancellationToken ct = default)
        {
            var requestParam = parameters.AsDictionary();
            switch (parameters.Type)
            {
                case WhiteBitOrderType.Market:
                case WhiteBitOrderType.StopMarket:
                case WhiteBitOrderType.ConditionalMarket:
                case WhiteBitOrderType.MarginMarket:
                case WhiteBitOrderType.MarginTriggerStopMarket:
                case WhiteBitOrderType.StockMarket:
                case WhiteBitOrderType.MarketStock:
                    requestParam.Add("nonce", Nonce);
                    break;
                default:
                    requestParam.Add("nonce", Nonce);//TODO just for test
                    break;
            }
            var result =  parameters.Type switch
            {
                WhiteBitOrderType.Limit => await SendRequestAsync<WhiteBitOrder>(PlaceLimitOrderUrl, ct, requestParam),
                WhiteBitOrderType.Market => await SendRequestAsync<WhiteBitOrder>(PlaceMarketOrderUrl, ct, requestParam),
                WhiteBitOrderType.StockMarket => await SendRequestAsync<WhiteBitOrder>(PlaceStockMarketOrderUrl, ct, requestParam),
                WhiteBitOrderType.StopMarket => await SendRequestAsync<WhiteBitOrder>(PlaceStopMarketOrderUrl, ct, requestParam),
                WhiteBitOrderType.StopLimit => await SendRequestAsync<WhiteBitOrder>(PlaceStopLimitOrderUrl, ct, requestParam),
                _ => throw new ArgumentException("Unsupported order type")
            };
            if (result)
            {
                OnOrderPlaced?.Invoke(new OrderId() { Id = result.Data.OrderId.ToString(), SourceObject = result.Data });
            }
            return result;
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<WhiteBitOrder>> CancelOrderAsync(string symbol, long orderId, CancellationToken ct = default)
        {
            var requestParam = new Dictionary<string, object>();
            requestParam.Add("market", symbol);
            requestParam.Add("orderId", orderId);
            var result  = await SendRequestAsync<WhiteBitOrder>(CancelOrderUrl, ct, requestParam);
            if (result)
            {
                OnOrderCanceled?.Invoke(new OrderId() { Id = result.Data.OrderId.ToString(), SourceObject = result.Data });
            }
            return result;
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<List<WhiteBitOrder>>> GetActiveOrdersAsync(GetActiveOrdersRequest request, CancellationToken ct = default)
        {
            WebCallResult<JToken> result = await SendRequestAsync<JToken>(ActiveOrdersUrl, ct, request.AsDictionary());
            if (result.Data is JArray array)
            {
                return result.As(array.ToObject<List<WhiteBitOrder>>()!);
            }
            return result.As<List<WhiteBitOrder>>(new List<WhiteBitOrder>() {result.Data?.ToObject<WhiteBitOrder>()! });
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<Dictionary<string, IEnumerable<WhiteBitOrder>>>> GetExecutedOrdersAsync(GetExecutedOrdersRequest? request = null, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<JToken>(FilledOrdersUrl, ct, request?.AsDictionary());
            if (result.Data is JArray array)
            {
                if (array.Count == 0)
                {
                    return result.As(new Dictionary<string, IEnumerable<WhiteBitOrder>>());
                }
                return result.As(new Dictionary<string, IEnumerable<WhiteBitOrder>>() { { request?.Market ?? string.Empty, array.ToObject<IEnumerable<WhiteBitOrder>>()!} });
            }
            return result.As<Dictionary<string, IEnumerable<WhiteBitOrder>>>(
                result.Data?.ToObject<Dictionary<string, IEnumerable<WhiteBitRawOrder>>>()?.ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value.Select(ord => ord.Convert(new() { Symbol = entry.Key })!)
                )
            );
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<Dictionary<string, IEnumerable<WhiteBitUserTrade>>>> GetOwnTradesAsync(GetOwnTradesRequest? request = null, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<Dictionary<string, IEnumerable<WhiteBitUserTrade>>>(OwnTradesUrl, ct, request?.AsDictionary());
            return result.As<Dictionary<string, IEnumerable<WhiteBitUserTrade>>>(
                result.Data?.ToDictionary(
                    entry => entry.Key,
                    entry => entry.Value.Select(tr => tr.Convert<WhiteBitUserTrade>(new() { Symbol = entry.Key })!)
                )
            );
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitUserTrade>?>> GetOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct = default)
        {
            var result = await SendRequestAsync<WhiteBitPaginatedResponse<IEnumerable<WhiteBitUserTrade>>>(OrderTradesUrl, ct, request.AsDictionary());
            return result.As(result.Data?.Result);
        }

        ///<inheritdoc/>
        public async Task<WebCallResult<GenerateNewAddress>> GetDepositeAddress (string symbol, string network = null,CancellationToken ct = default)
        {
            var requestParam = new Dictionary<string, object>();
            requestParam.Add("ticker", symbol);
            requestParam.Add("network", network);
            return await SendRequestAsync<GenerateNewAddress>(DepositeAddress, ct, requestParam);
        }

        ///<inheritdoc/>
        public async Task<WebCallResult<GenerateNewAddress>> GenerateNewAddress(string symbol, string network = null, CancellationToken ct = default)
        {
            var requestParam = new Dictionary<string, object>();
            requestParam.Add("ticker", symbol);
            requestParam.Add("network", network);
            return await SendRequestAsync<GenerateNewAddress>(CreateDepositeAddress, ct, requestParam);
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<ResponseWithdrawError?>> WithdrawRequest(string syymbol,decimal amount,string address,string uniqueId, string memo = "",string network = "",bool isFee =false, CancellationToken ct = default)
        {
            var param = new Dictionary<string, object>();
            param.Add("ticker", syymbol);
            param.Add("amount", amount.ToString("G", CultureInfo.InvariantCulture));
            param.Add("address", address);
            if(!string.IsNullOrEmpty(memo))
               param.Add("memo", memo);
            param.Add("uniqueId", uniqueId);
            param.Add("network", network);
            var requestUrl = WithdrawRequst;
            if (isFee)
            {
                requestUrl = WithdrawPayRequst;
            }
            return await SendRequestAsync<ResponseWithdrawError?>(requestUrl, ct, param);
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<ResponseTransferAmountError?>> TransferAmount(string from,string to,string token,decimal amount,CancellationToken ct = default)
        {
            var param = new Dictionary<string, object>();
            param.Add("ticker", token);
            param.Add("from", from);
            param.Add("to", to);
            param.Add("amount", amount.ToString("G", CultureInfo.InvariantCulture));
            return await SendRequestAsync<ResponseTransferAmountError?>(GetTransferRequest,ct,param);
            
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<WhitdrawalDepositHistoryResponse>> GetDepositeWithdrawalHistory(WhitdrawalDepositHistoryRequest model,CancellationToken ct = default)
        {
            return await SendRequestAsync<WhitdrawalDepositHistoryResponse>(GetDepositeWsitrawalHistory, ct, model?.AsDictionary());
        }
        #endregion

        #region RestApiClient methods
        public async Task<WebCallResult<OrderId?>> CancelOrderAsync(string orderId, string symbol, CancellationToken ct = default)
        {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var result = await CancelOrderAsync(symbol, Int64.Parse(orderId), ct);
            return result.As(!result ? null : new OrderId() { Id = result.Data.OrderId.ToString(), SourceObject = result.Data });
        }

        async Task<WebCallResult<IEnumerable<Balance>?>> IBaseRestClient.GetBalancesAsync(string? accountId, CancellationToken ct)
        {
            var result = await GetBalancesAsync(ct);
            return result.As(result.Data?.Select(b => b.Convert(new Balance() {SourceObject = b })!));
        }

        async Task<WebCallResult<IEnumerable<Order>?>> IBaseRestClient.GetClosedOrdersAsync(string? symbol, CancellationToken ct)
        {
            var result = await GetExecutedOrdersAsync(symbol is null ? null : new GetExecutedOrdersRequest(symbol), ct);
            return result.As(result.Data?.SelectMany(x => x.Value).Select(ord => ord.ToCryptoExchangeOrder()));
        }
        /// <summary>
        /// Unimplemented yet
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<Kline>>> IBaseRestClient.GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime, DateTime? endTime, int? limit, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public async Task<WebCallResult<IEnumerable<Order>?>> GetOpenOrdersAsync(string symbol, CancellationToken ct = default)
        {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var result = await GetActiveOrdersAsync(new GetActiveOrdersRequest(symbol), ct);
            return result.As(result.Data?.Select(ord => ord.ToCryptoExchangeOrder()));
        }

        public async Task<WebCallResult<Order?>> GetOrderAsync(string orderId, string symbol, CancellationToken ct = default)
        {
            if (symbol == null) throw new ArgumentNullException(nameof(symbol));

            var resultA = await GetActiveOrdersAsync(new GetActiveOrdersRequest(symbol, Int64.Parse(orderId)), ct);

            if (resultA.Data?.Any() == true)
            {
                var orderA = resultA.Data.Last();
                return resultA.As(orderA?.ToCryptoExchangeOrder());
            }
            var resultX = await GetExecutedOrdersAsync(new GetExecutedOrdersRequest(symbol, Int64.Parse(orderId)), ct);
            var orderX = resultX.Data?.Any() == true ? resultX.Data?[symbol].LastOrDefault() : null;
            return resultX.As(orderX?.ToCryptoExchangeOrder());
        }

        async Task<WebCallResult<OrderBook?>> IBaseRestClient.GetOrderBookAsync(string symbol, CancellationToken ct)
        {
            var result = await GetOrderBookAsync(symbol, ct: ct);
            return result.As(result.Data?.ToCryptoExchangeOrderBook());
        }

        async Task<WebCallResult<IEnumerable<UserTrade>?>> IBaseRestClient.GetOrderTradesAsync(string orderId, string? symbol, CancellationToken ct)
        {
            var result = await GetOrderTradesAsync(new GetOrderTradesRequest(Int64.Parse(orderId)), ct);
            return result.As(result.Data?.Select(trade => trade.Convert(new UserTrade() { SourceObject = trade })!));
        }

        async Task<WebCallResult<IEnumerable<Trade>?>> IBaseRestClient.GetRecentTradesAsync(string symbol, CancellationToken ct)
        {
            var result = await GetPublicTradesAsync(symbol, null , ct);
            return result.As(result.Data?.Select(trade => trade.Convert(new Trade() { SourceObject = trade })!));
        }

        public string GetSymbolName(string baseAsset, string quoteAsset)
        {
            return $"{baseAsset}_{quoteAsset}".ToUpper(CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// Unimplemented yet
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<Symbol>>> IBaseRestClient.GetSymbolsAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unimplemented yet
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<Ticker>> IBaseRestClient.GetTickerAsync(string symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        async Task<WebCallResult<IEnumerable<Ticker>?>> IBaseRestClient.GetTickersAsync(CancellationToken ct)
        {
            var result = await GetTickersAsync(ct);
            return result.As(result.Data?.Select(t => t.ToCryptoExchangeTicker()));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="side"></param>
        /// <param name="type">supported Limit or Market only</param>
        /// <param name="quantity"></param>
        /// <param name="price">required for Limit order</param>
        /// <param name="accountId"></param>
        /// <param name="clientOrderId"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        async public Task<WebCallResult<OrderId?>> PlaceOrderAsync(string symbol, CommonOrderSide side, CommonOrderType type, decimal quantity, decimal? price = null, string? accountId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            WhiteBitPlaceOrderRequest request = type switch
            {
                CommonOrderType.Market => WhiteBitPlaceOrderRequest.CreateMarketOrderRequest(symbol, side.ToWhiteBitOrderSide(), quantity, clientOrderId),
                CommonOrderType.Limit when price is null => throw new ArgumentNullException("price should not be null for Limit order"),
                CommonOrderType.Limit => WhiteBitPlaceOrderRequest.CreateLimitOrderRequest(symbol, side.ToWhiteBitOrderSide(), quantity, price!.Value, clientOrderId),
                _ => throw new ArgumentException("Unsupported order type, use either Market or Limit")
            };
            var result = await PlaceOrderAsync(request, ct);
            return result.As(!result ? null : new OrderId() { Id = result.Data.OrderId.ToString(), SourceObject = result.Data });
        }
        #endregion

        #region RestApiClient methods
        public override TimeSpan? GetTimeOffset() => TimeSyncState.TimeOffset;

        /// <inheritdoc />
        public override TimeSyncInfo GetTimeSyncInfo() => new TimeSyncInfo(_logger, Options.AutoTimestamp, Options.TimestampRecalculationInterval, TimeSyncState);

        protected override async Task<WebCallResult<DateTime>> GetServerTimestampAsync()
        {
            var result = await SendRequestAsync<WhiteBitServerTime>(ServerTimeUrl);
            return result.As(result.Data?.Time ?? default);
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        {
            return new WhiteBitAuthenticationProvider(credentials);
        }
        #endregion
        
        private async Task<WebCallResult<T>> SendRequestAsync<T>(string endpoint, CancellationToken ct = default, Dictionary<string, object>? request = null) where T : class
        {
            var isPublic = endpoint.IndexOf("public") > -1;
            return await SendRequestInternal<T>(
                GetUrl(endpoint),
                isPublic ? HttpMethod.Get : HttpMethod.Post,
                ct,
                request,
                AuthenticationProvider is not null,
                null,
                isPublic ? HttpMethodParameterPosition.InUri : HttpMethodParameterPosition.InBody
            );
        }
    }
}