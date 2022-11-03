using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Interfaces;
using Newtonsoft.Json;
using WhiteBit.Net.Helpers;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitOrderBook : WhiteBitBaseOrderBook
    {
        /// <summary>
        /// Current timestamp
        /// </summary>
        [JsonProperty("timestamp")]
        public long Timestamp { get; set; }

        internal OrderBook ToCryptoExchangeOrderBook()
        {
            return new OrderBook()
            {
                SourceObject = this,
                Asks = Asks
                    .Select(entry => new OrderBookEntry()
                    {
                        Price = entry.Price,
                        Quantity = entry.Quantity
                    }),
                Bids = Bids
                    .Select(entry => new OrderBookEntry()
                    {
                        Price = entry.Price,
                        Quantity = entry.Quantity
                    })
            };
        }
    }

    public class WhiteBitBaseOrderBook
    {
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

    [JsonConverter(typeof(ArrayConverter))]
    public class WhiteBitSocketOrderBook
    {
        //true - full reload, false - partial update
        [ArrayProperty(0)]
        public bool IsFullReload { get; set; }

        //for partial update - finished orders will be[price, "0"]
        [ArrayProperty(1), JsonConverter(typeof(ObjectJsonConverter<WhiteBitBaseOrderBook>))]
        public WhiteBitBaseOrderBook? Book { get; set; }

        [ArrayProperty(2)]
        string Symbol { get; set; } = string.Empty;

        public IEnumerable<WhiteBitOrderBookEntry> Bids => Book?.Bids ?? Array.Empty<WhiteBitOrderBookEntry>();
        public IEnumerable<WhiteBitOrderBookEntry> Asks => Book?.Asks ?? Array.Empty<WhiteBitOrderBookEntry>();
    }
}