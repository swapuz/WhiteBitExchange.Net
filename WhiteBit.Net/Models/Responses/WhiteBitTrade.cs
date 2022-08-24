using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitTrade
    {
        /// <summary>
        /// deal ID
        /// </summary>
        [JsonProperty("id")]
        public long Id { get; set; }

        /// <summary>
        /// custom order id; "clientOrderId": "" - if not specified.
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; } = string.Empty;

        /// <summary>
        /// Timestamp of the executed deal
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Time { get; set; }

        /// <summary>
        /// Deal side "sell" / "buy"
        /// </summary>
        [JsonProperty("side")]
        public WhiteBitOrderSide Side { get; set; }

        /// <summary>
        /// Role - maker or taker
        /// </summary>
        [JsonProperty("role")]
        public TraderRole Role { get; set; }

        /// <summary>
        /// amount in stock
        /// </summary>
        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// amount in money
        /// </summary>
        [JsonProperty("deal")]
        public decimal Deal { get; set; }

        /// <summary>
        /// paid fee
        /// </summary>
        [JsonProperty("fee")]
        public decimal Fee { get; set; }
    }
}