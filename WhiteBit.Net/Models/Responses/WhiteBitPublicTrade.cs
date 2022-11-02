using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitPublicTrade : IConvertible<WhiteBitPublicTrade>,IConvertible<Trade>
    {
        private decimal baseVolume;
        private decimal quoteVolume;
        private DateTime timestamp;
        private WhiteBitOrderSide? side;
        private long tradeId;

        [JsonProperty("market")]
        public string? Symbol { get; set; }

        /// <summary>
        /// A unique ID associated with the trade for the currency pair transaction Note: Unix timestamp does not qualify as trade_id.
        /// </summary>
        [JsonProperty("tradeID")]
        public long TradeId { get => tradeId; set => tradeId = value; }
        [JsonProperty("id")]
        internal long TradeId0 {set => tradeId = value; }

        /// <summary>
        /// Transaction price in quote pair volume.
        /// </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Transaction amount in base pair volume.
        /// </summary>
        [JsonProperty("base_volume")]
        public decimal BaseVolume { get => baseVolume; set => baseVolume = value; }

        /// <summary>
        /// amount in stock
        /// </summary>
        [JsonProperty("amount")]
        internal decimal Amount { set => baseVolume = value; }

        /// <summary>
        /// Transaction amount in quote pair volume.
        /// </summary>
        [JsonProperty("quote_volume")]
        public decimal QuoteVolume { get => quoteVolume; set => quoteVolume = value; }

        /// <summary>
        /// Identifies when the transaction occurred.
        /// </summary>
        [JsonProperty("trade_timestamp"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get => timestamp; set => timestamp = value; }

        /// <summary>
        /// Timestamp of the executed deal
        /// </summary>
        [JsonProperty("time")]
        [JsonConverter(typeof(DateTimeConverter))]
        internal DateTime Timestamp0 { set => timestamp = value; }

        /// <summary>
        /// Used to determine whether or not the transaction originated as a buy or sell. 
        /// Buy – Identifies an ask that was removed from the order book. 
        /// Sell – Identifies a bid that was removed from the order book.
        /// </summary>
        [JsonProperty("type")]
        public WhiteBitOrderSide? Side { get => side; set => side = value; }



        #region CryptoExchange.Net.CommonObjects
        protected decimal Quantity => BaseVolume;

        #endregion
    }

    [JsonConverter(typeof(ArrayConverter))]
    internal class publicTradesAsArray
    {
        [ArrayProperty(0)]
        public string Symbol { get; set; } = string.Empty;
        [ArrayProperty(1), JsonConverter(typeof(ObjectJsonConverter<List<WhiteBitPublicTrade>>))]
        public IEnumerable<WhiteBitPublicTrade> Body { get; set; } = Array.Empty<WhiteBitPublicTrade>();
    }
}