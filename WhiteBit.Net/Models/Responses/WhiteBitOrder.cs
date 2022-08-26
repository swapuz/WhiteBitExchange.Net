using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitOrder : WhiteBitRawOrder
    {
        /// <summary>
        /// deal market
        /// </summary>
        [JsonProperty("market")]
        public string Symbol { get; set; } = string.Empty;
    }
    public class WhiteBitRawOrder : IConvertible<WhiteBitOrder>
    {
        private long orderId;
        private string clientOrderId = string.Empty;
        private decimal dealStock;
        private decimal dealMoney;
        private decimal dealFee;
        private DateTime creatingTimestamp;
        private DateTime lastUpdateTimestamp;

        /// <summary>
        /// order id
        /// </summary>
        [JsonProperty("orderId")]
        public long OrderId { get => orderId; set => orderId = value; }
        /// <summary>
        ///  additional prop for one vore alias
        /// </summary>
        [JsonProperty("id")]
        internal long OrderIdAsIdProp { set => orderId = value; }
        /// <summary>
        /// custom client order id; "clientOrderId": "" - if not specified.
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get => clientOrderId; set => clientOrderId = value; }
        /// <summary>
        /// additional prop for one vore alias
        /// </summary>
        [JsonProperty("client_order_id")]
        internal string ClientOrderIdWithUnderscores { set => clientOrderId = value; }


        /// <summary>
        /// order side
        /// </summary>
        [JsonProperty("side")]
        public WhiteBitOrderSide Side { get; set; }

        [JsonProperty("type")]
        public WhiteBitOrderType Type { get; set; }

        /// <summary>
        /// amount in stock currency that finished
        /// </summary>
        [JsonProperty("dealMoney")]
        public decimal DealMoney { get => dealMoney; set => dealMoney = value; }
        [JsonProperty("deal_money")]
        internal decimal DealMoneyUnderscore { set => dealMoney = value; }

        /// <summary>
        /// amount in stock currency that finished
        /// </summary>
        [JsonProperty("dealStock")]
        public decimal DealStock { get => dealStock; set => dealStock = value; }

        [JsonProperty("deal_stock")]
        internal decimal DealStockUpderscore { set => dealStock = value; }

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
        ///  fee in money that you pay if order is finished
        /// </summary>
        [JsonProperty("dealFee")]
        public decimal DealFee { get => dealFee; set => dealFee = value; }
        [JsonProperty("deal_fee")]
        internal decimal DealFeeUnderScore { set => dealFee = value; }

        [JsonProperty("price")]
        public decimal? Price { get; set; }


        [JsonProperty("timestamp")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime CreatingTimestamp
        {
            get => creatingTimestamp;
            set
            {
                creatingTimestamp = value;
                LastUpdateTimestamp = value;    // update lastUpdateTimestamp too if needed
            }
        }

        [JsonProperty("ctime")]
        [JsonConverter(typeof(DateTimeConverter))]
        internal DateTime CreatingCTime { set => CreatingTimestamp = value; }   // update lastUpdateTimestamp too if needed

        [JsonProperty("ftime")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime LastUpdateTimestamp
        {
            get => lastUpdateTimestamp;
            set
            {
                if (lastUpdateTimestamp < value)
                    lastUpdateTimestamp = value;
            }
        }
        [JsonProperty("mtime")]
        [JsonConverter(typeof(DateTimeConverter))]
        internal DateTime MTimestamp {set => LastUpdateTimestamp = value; }


        /// <summary>
        ///  activation price
        /// </summary>
        [JsonProperty("activation_price")]
        public decimal? ActivationPrice { get; set; }

        /// <summary>
        /// rest of amount that must be finished
        /// </summary>
        [JsonProperty("left")]
        public decimal Left { get; set; }
    }
}