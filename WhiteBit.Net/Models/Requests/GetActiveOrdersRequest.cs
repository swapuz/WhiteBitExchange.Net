using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Requests
{
    public class GetActiveOrdersRequest : PaginationRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="market"><see cref="Market"/></param>
        /// <param name="orderId"><see cref="OrderId"/></param>
        /// <param name="clientOrderId"><see cref="ClientOrderId"/></param>
        /// <param name="limit"><see cref="PaginationRequest.Limit"/></param>
        /// <param name="offset"><see cref="PaginationRequest.Offset"/></param>
        public GetActiveOrdersRequest(string market, long? orderId = null, string? clientOrderId = null, int? limit = null, int? offset = null) : base(limit, offset)
        {
            Market = market;
            OrderId = orderId;
            ClientOrderId = clientOrderId;
        }

        [JsonProperty("market")]
        public string Market { get; set; }

        [JsonProperty("orderId")]
        public long? OrderId { get; set; }

        [JsonProperty("clientOrderId")]
        public string? ClientOrderId { get; set; }
    }
}