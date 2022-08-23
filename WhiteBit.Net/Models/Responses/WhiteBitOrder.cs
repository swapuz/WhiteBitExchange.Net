using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitOrder
    {
        /// <summary>
        /// order id
        /// </summary>
        [JsonProperty("orderId")]
        public long OrderId { get; set; }
        /// <summary>
        /// custom client order id; "clientOrderId": "" - if not specified.
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; } = string.Empty;

        /// <summary>
        /// deal market
        /// </summary>
        [JsonProperty("market")]
        public string Market { get; set; } = string.Empty;

        /// <summary>
        /// order side
        /// </summary>
        [JsonProperty("side")]
        public WhiteBitOrderSide Side { get; set; }

        [JsonProperty("type")]
        public WhiteBitOrderType Type { get; set; }

        [JsonProperty("timestamp")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// amount in stock currency that finished
        /// </summary>
        [JsonProperty("dealMoney")]
        public decimal DealMoney { get; set; }

        /// <summary>
        /// amount in stock currency that finished
        /// </summary>
        [JsonProperty("dealStock")]
        public decimal DealStock { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// maker fee ratio. If the number less than 0.0001 - its rounded to zero
        /// </summary>
        [JsonProperty("takerFee")]
        public decimal TakerFee { get; set; }

        /// <summary>
        /// maker fee ratio. If the number less than 0.0001 - its rounded to zero
        /// </summary>
        [JsonProperty("makerFee")]
        public decimal MakerFee { get; set; }

        /// <summary>
        /// rest of amount that must be finished
        /// </summary>
        [JsonProperty("left")]
        public decimal Left { get; set; }

        /// <summary>
        ///  fee in money that you pay if order is finished
        /// </summary>
        [JsonProperty("dealFee")]
        public decimal DealFee { get; set; }
        /// <summary>
        ///  activation price
        /// </summary>
        [JsonProperty("activation_price")]
        public decimal? ActivationPrice { get; set; }


        [JsonProperty("price")]
        public decimal? Price { get; set; }
    }
}