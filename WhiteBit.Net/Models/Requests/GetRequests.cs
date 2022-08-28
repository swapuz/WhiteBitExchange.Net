using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Requests
{
    public class BaseGetRequest : PaginationRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="market"><see cref="Market"/></param>
        /// <param name="orderId"><see cref="OrderId"/></param>
        /// <param name="clientOrderId"><see cref="ClientOrderId"/></param>
        /// <param name="limit"><see cref="PaginationRequest.Limit"/></param>
        /// <param name="offset"><see cref="PaginationRequest.Offset"/></param>
        protected BaseGetRequest(string? market = null, long? orderId = null, string? clientOrderId = null, int? limit = null, int? offset = null) : base(limit, offset)
        {
            Market = market;
            OrderId = orderId;
            ClientOrderId = clientOrderId;
        }

        [JsonProperty("market")]
        public string? Market { get; private set; }

        [JsonProperty("orderId")]
        public long? OrderId { get; private set; }

        [JsonProperty("clientOrderId")]
        public string? ClientOrderId { get; private set; }

    }

    public class GetActiveOrdersRequest : BaseGetRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="orderId">Available orderId. Example: 3134995325</param>
        /// <param name="clientOrderId">Available clientOrderId. Example: customId11</param>
        /// <param name="limit">LIMIT is a special clause used to limit records a particular query can return. Default: 50, Min: 1, Max: 100</param>
        /// <param name="offset">If you want the request to return entries starting from a particular line, you can use OFFSET clause to tell it where it should start. Default: 0, Min: 0, Max: 10000</param>
        public GetActiveOrdersRequest(string symbol, long? orderId = null, string? clientOrderId = null, int? limit = null, int? offset = null) : base(symbol, orderId, clientOrderId, limit, offset)
        {}
    }
    public class GetExecutedOrdersRequest : BaseGetRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="orderId">Available orderId. Example: 3134995325</param>
        /// <param name="clientOrderId">Available clientOrderId. Example: customId11</param>
        /// <param name="limit">LIMIT is a special clause used to limit records a particular query can return. Default: 50, Min: 1, Max: 100</param>
        /// <param name="offset">If you want the request to return entries starting from a particular line, you can use OFFSET clause to tell it where it should start. Default: 0, Min: 0, Max: 10000</param>
        public GetExecutedOrdersRequest(string? symbol = null, long? orderId = null, string? clientOrderId = null, int? limit = null, int? offset = null) : base(symbol, orderId, clientOrderId, limit, offset)
        {}
    }
    public class GetOrderTradesRequest : BaseGetRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="orderId">Available orderId. Example: 3134995325</param>
        /// <param name="limit">LIMIT is a special clause used to limit records a particular query can return. Default: 50, Min: 1, Max: 100</param>
        /// <param name="offset">If you want the request to return entries starting from a particular line, you can use OFFSET clause to tell it where it should start. Default: 0, Min: 0, Max: 10000</param>
        public GetOrderTradesRequest(long orderId, int? limit = null, int? offset = null) : base(orderId:orderId, limit:limit, offset:offset)
        {}
    }
    public class GetOwnTradesRequest : BaseGetRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="clientOrderId">Available clientOrderId. Example: customId11</param>
        /// <param name="limit">LIMIT is a special clause used to limit records a particular query can return. Default: 50, Min: 1, Max: 100</param>
        /// <param name="offset">If you want the request to return entries starting from a particular line, you can use OFFSET clause to tell it where it should start. Default: 0, Min: 0, Max: 10000</param>
        public GetOwnTradesRequest(string? symbol = null, string? clientOrderId = null, int? limit = null, int? offset = null) : base(symbol, null, clientOrderId, limit, offset)
        { }
    }

    
}