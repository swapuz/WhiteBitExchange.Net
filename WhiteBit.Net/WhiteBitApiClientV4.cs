using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net
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
        private const string BalanceUrl = "trade-account/balance";
        private const string PlaceLimitOrderUrl = "order/new";
        private const string PlaceStopLimitOrderUrl = "order/stop_limit";
        private const string PlaceMarketOrderUrl = "order/market";
        private const string PlaceStopMarketOrderUrl = "order/stop_market";
        private const string PlaceStockMarketOrderUrl = "order/stock_market";
        private const string CancelOrderUrl = "order/cancel";
        private const string ActiveOrdersUrl = "orders";
        private const string FilledOrdersUrl = "trade-account/order/history";
        private const string OrderHistoryUrl = "trade-account/order";


        #endregion
        public WhiteBitApiClientV4(string name, BaseRestClientOptions options, RestApiClientOptions apiOptions, Log log, WhiteBitClient client) : base(name, options, apiOptions, log, client)
        {
        }

        protected override string ApiVersion => "4";
        internal static TimeSyncState TimeSyncState = new TimeSyncState("Spot Api");


        public event Action<OrderId>? OnOrderPlaced;
        public event Action<OrderId>? OnOrderCanceled;

        #region IWhiteBitApiClientV4 Methods
        ///<inheritdoc/>
        public async Task<WebCallResult<WhiteBitTradingBalance>> GetBalanceAsync(string currency, CancellationToken ct = default)
        {
            currency = currency.ToUpper();
            var result =  await SendRequestAsync<WhiteBitRawTradingBalance>(BalanceUrl, ct, new Dictionary<string, object>{{"ticker", currency}});
            return result.As(result.Data.Convert(new WhiteBitTradingBalance {Currency = currency}));
        }
        
        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitTradingBalance>>> GetBalancesAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<Dictionary<string, WhiteBitRawTradingBalance>>(BalanceUrl, ct);
            return result.As(result.Data.Select(b => b.Value.Convert(new WhiteBitTradingBalance { Currency = b.Key })));
        }

        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitTicker>>> GetTickersAsync(CancellationToken ct = default)
        {
            var result =  await SendRequestAsync<Dictionary<string,WhiteBitRawTicker>>(TickerUrl, ct);
            return result.As(result.Data.Select(b => b.Value.Convert(new WhiteBitTicker { Symbol = b.Key })));
        }

        ///<inheritdoc/>
        public async Task<WebCallResult<IEnumerable<WhiteBitAsset>>> GetAssetsAsync(CancellationToken ct = default)
        {
            var result = await SendRequestAsync<Dictionary<string, WhiteBitRawAsset>>(AssetsUrl, ct);
            // return result.As(result.Data.Select(b => new WhiteBitAsset(b.Value) { Currency = b.Key }));
            return result.As(result.Data.Select(b => b.Value.Convert(new WhiteBitAsset {Currency = b.Key} )));
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
        public async Task<WebCallResult<List<WhiteBitPublicTrade>>> GetPublicTradesAsync(string symbol, WhiteBitOrderSide? side = null, CancellationToken ct = default)
        {
            var param = new Dictionary<string, object>();
            param.AddOptionalParameter("type", side?.ToString().ToLower());
            return await SendRequestAsync<List<WhiteBitPublicTrade>>(FillPathParameter(PublicTradesUrl, symbol), ct, param);
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<List<WhiteBitFutures>?>> GetFuturesAsync(CancellationToken ct = default)
        {
            var result =  await SendRequestAsync<BaseResponse<List<WhiteBitFutures>>>(FuturesListUrl, ct);
            return result.As(result.Data?.Result);
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
            return parameters.Type switch
            {
                WhiteBitOrderType.Limit => await SendRequestAsync<WhiteBitOrder>(PlaceLimitOrderUrl, ct, requestParam),
                WhiteBitOrderType.Market => await SendRequestAsync<WhiteBitOrder>(PlaceMarketOrderUrl, ct, requestParam),
                WhiteBitOrderType.StockMarket => await SendRequestAsync<WhiteBitOrder>(PlaceStockMarketOrderUrl, ct, requestParam),
                WhiteBitOrderType.StopMarket => await SendRequestAsync<WhiteBitOrder>(PlaceStopMarketOrderUrl, ct, requestParam),
                WhiteBitOrderType.StopLimit => await SendRequestAsync<WhiteBitOrder>(PlaceStopLimitOrderUrl, ct, requestParam),
                _ => throw new ArgumentException("Unsupported order type")
            };
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<WhiteBitOrder>> CancelOrderAsync(string symbol, long orderId, CancellationToken ct = default)
        {
            var requestParam = new Dictionary<string, object>();
            requestParam.Add("market", symbol);
            requestParam.Add("orderId", orderId);
            return await SendRequestAsync<WhiteBitOrder>(CancelOrderUrl, ct, requestParam);
        }
        ///<inheritdoc/>
        public async Task<WebCallResult<List<WhiteBitOrder>>> GetActiveOrdersAsync(GetActiveOrdersRequest request, CancellationToken ct = default)
        {
            WebCallResult<JToken> result = await SendRequestAsync<JToken>(ActiveOrdersUrl, ct, request.AsDictionary());
            if (result.Data is JArray array)
            {
                return result.As(array.ToObject<List<WhiteBitOrder>>()!);
            }
            return result.As<List<WhiteBitOrder>>(new List<WhiteBitOrder>() {result.Data.ToObject<WhiteBitOrder>()! });
        }
        // ///<inheritdoc/>
        // public async Task<WebCallResult<List<WhiteBitOrder>>> GetFilledOrdersAsync(GetFilledOrdersRequest request, CancellationToken ct = default)
        // {
        //     WebCallResult<JToken> result = await SendRequestAsync<JToken>(ActiveOrdersUrl, ct, request.AsDictionary());
        //     if (result.Data is JArray array)
        //     {
        //         return result.As(array.ToObject<List<WhiteBitOrder>>()!);
        //     }
        //     return result.As<List<WhiteBitOrder>>(new List<WhiteBitOrder>() {result.Data.ToObject<WhiteBitOrder>()! });
        // }

        #endregion

        #region RestApiClient methods
        Task<WebCallResult<OrderId>> IBaseRestClient.CancelOrderAsync(string orderId, string? symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<Balance>>> GetBalancesAsync(string? accountId = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<Order>>> GetClosedOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<Kline>>> GetKlinesAsync(string symbol, TimeSpan timespan, DateTime? startTime = null, DateTime? endTime = null, int? limit = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<Order>>> GetOpenOrdersAsync(string? symbol = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<Order>> GetOrderAsync(string orderId, string? symbol = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<OrderBook>> GetOrderBookAsync(string symbol, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<UserTrade>>> GetOrderTradesAsync(string orderId, string? symbol = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<Trade>>> GetRecentTradesAsync(string symbol, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public string GetSymbolName(string baseAsset, string quoteAsset)
        {
            return $"{baseAsset}_{quoteAsset}".ToUpper(CultureInfo.InvariantCulture);
        }

        public Task<WebCallResult<IEnumerable<Symbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        Task<WebCallResult<Ticker>> IBaseRestClient.GetTickerAsync(string symbol, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        Task<WebCallResult<IEnumerable<Ticker>>> IBaseRestClient.GetTickersAsync(CancellationToken ct)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<OrderId>> PlaceOrderAsync(string symbol, CommonOrderSide side, CommonOrderType type, decimal quantity, decimal? price = null, string? accountId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region RestApiClient methods
        public override TimeSpan GetTimeOffset() => TimeSyncState.TimeOffset;

        /// <inheritdoc />
        public override TimeSyncInfo GetTimeSyncInfo() => new TimeSyncInfo(log,Options.AutoTimestamp, Options.TimestampRecalculationInterval, TimeSyncState);

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
            return await baseClient.SendRequestInternal<T>(
                this,
                GetUrl(endpoint),
                isPublic ? HttpMethod.Get : HttpMethod.Post,
                ct,
                request,
                AuthenticationProvider is not null,
                isPublic ? HttpMethodParameterPosition.InUri : HttpMethodParameterPosition.InBody
            );
        }

        #region test
        public async Task<DateTime> GetServerTimestampTest()
        {
            return (await GetServerTimestampAsync()).Data;
        }
        #endregion
    }
}