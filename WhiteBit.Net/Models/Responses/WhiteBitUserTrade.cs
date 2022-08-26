using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitUserTrade : WhiteBitPublicTrade, IConvertible<UserTrade>, IConvertible<WhiteBitUserTrade>
    {
        private long? orderId;

        /// <summary>
        /// custom order id; "clientOrderId": "" - if not specified.
        /// </summary>
        [JsonProperty("clientOrderId")]
        public string ClientOrderId { get; set; } = string.Empty;

        /// <summary>
        /// Order ID
        /// </summary>
        [JsonProperty("deal_order_id")]
        public long? OrderId { get => orderId; set => orderId = value; }
        [JsonProperty("dealOrderId")]
        internal long? OrderId0 { set => orderId = value; }
        [JsonProperty("orderId")]
        internal long? OrderId1 { set => orderId = value; }

        /// <summary>
        /// Deal side "sell" / "buy"
        /// </summary>
        [JsonProperty("side")]
        internal WhiteBitOrderSide Side0 { set => Side = value; }

        /// <summary>
        /// Role - maker or taker
        /// </summary>
        [JsonProperty("role")]
        public TraderRole Role { get; set; }


        /// <summary>
        /// amount in money
        /// </summary>
        [JsonProperty("deal")]
        internal decimal Deal { set =>  QuoteVolume = value; }

        /// <summary>
        /// paid fee
        /// </summary>
        [JsonProperty("fee")]
        public decimal Fee { get; set; }


        #region CryptoExchange.Net.CommonObjects
        //
        // Summary:
        //     Id of the trade
        private string Id => TradeId.ToString();

        //
        // Summary:
        //     The asset the fee is paid in
        private string? FeeAsset => Symbol?.Split('_').LastOrDefault();
        #endregion
    }
    public class OrderTradesResponse : WhiteBitPaginatedResponse<IEnumerable<WhiteBitUserTrade>>
    {

    }
}