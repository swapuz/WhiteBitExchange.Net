using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitPublicTrade
    {
        /// <summary>
        /// A unique ID associated with the trade for the currency pair transaction Note: Unix timestamp does not qualify as trade_id.
        /// </summary>
        [JsonProperty("tradeID")]
        public long TradeId { get; set; }

        /// <summary>
        /// Transaction price in quote pair volume.
        /// </summary>
        [JsonProperty("price")]
        public decimal Price { get; set; }

        /// <summary>
        /// Transaction amount in base pair volume.
        /// </summary>
        [JsonProperty("base_volume")]
        public decimal BaseVolume { get; set; }

        /// <summary>
        /// Transaction amount in quote pair volume.
        /// </summary>
        [JsonProperty("quote_volume")]
        public decimal QuoteVolume { get; set; }

        /// <summary>
        /// Identifies when the transaction occurred.
        /// </summary>
        [JsonProperty("trade_timestamp"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime TradeTimestamp { get; set; }

        /// <summary>
        /// Used to determine whether or not the transaction originated as a buy or sell. 
        /// Buy – Identifies an ask that was removed from the order book. 
        /// Sell – Identifies a bid that was removed from the order book.
        /// </summary>
        [JsonProperty("type")]
        public WhiteBitOrderSide Type { get; set; }
    }
}