using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net
{
    public class WhiteBitApiClientV4 : WhiteBitApiClient, IWhiteBitApiClientV4
    {

        public WhiteBitApiClientV4(string name, BaseRestClientOptions options, RestApiClientOptions apiOptions, Log log, WhiteBitClient client) : base(name, options, apiOptions, log, client)
        {
        }

        protected override string ApiVersion => "4";

        public event Action<OrderId>? OnOrderPlaced;
        public event Action<OrderId>? OnOrderCanceled;


        internal Uri GetUrl(string endpoint)
        {
            return new Uri(BaseAddress.AppendPath($"v{ApiVersion}"));
        }


        public Task<WebCallResult<OrderId>> CancelOrderAsync(string orderId, string? symbol = null, CancellationToken ct = default)
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
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<Symbol>>> GetSymbolsAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<Ticker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<IEnumerable<Ticker>>> GetTickersAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public override TimeSpan GetTimeOffset()
        {
            throw new NotImplementedException();
        }

        public override TimeSyncInfo GetTimeSyncInfo()
        {
            throw new NotImplementedException();
        }

        public Task<WebCallResult<OrderId>> PlaceOrderAsync(string symbol, CommonOrderSide side, CommonOrderType type, decimal quantity, decimal? price = null, string? accountId = null, string? clientOrderId = null, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
        {
            throw new NotImplementedException();
        }

        protected override Task<WebCallResult<DateTime>> GetServerTimestampAsync()
        {
            throw new NotImplementedException();
        }
    }
}