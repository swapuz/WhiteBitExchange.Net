using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitOrderBook
    {
        /// <summary>
        /// Current timestamp
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        /// <summary>
        /// Collection of ask orders, first is the best one
        /// </summary>
        [JsonProperty("asks")]
        public IEnumerable<WhiteBitOrderBookEntry> Asks { get; set; } = Array.Empty<WhiteBitOrderBookEntry>();

        /// <summary>
        /// Collection of bidds orders, first is the best one
        /// </summary>
        [JsonProperty("bids")]
        public IEnumerable<WhiteBitOrderBookEntry> Bids { get; set; } = Array.Empty<WhiteBitOrderBookEntry>();
    }

    [JsonConverter(typeof(ArrayConverter))]
    public class WhiteBitOrderBookEntry : ISymbolOrderBookEntry
    {
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
    }
}