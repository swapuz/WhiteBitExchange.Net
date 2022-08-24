using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using Newtonsoft.Json;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitTicker : WhiteBitRawTicker
    {

        public string? Symbol { get; set; }

        public Ticker ToCryptoExchangeTicker()
        {
            return new Ticker()
            {
                SourceObject = this,
                Symbol = this.Symbol!,
                LastPrice = this.LastPrice,
                Volume = this.Volume
            };
        }
    }
    public class WhiteBitRawTicker : IConvertible<WhiteBitTicker>
    {

        /// <summary>
        /// CoinmarketCap Id of base currency; 0 - if unknown
        /// </summary>
        [JsonProperty("base_id")]
        public int CoinmarketCapBaseId { get; set; }

        /// <summary>
        /// CoinmarketCap Id of quote currency; 0 - if unknown
        /// </summary>
        [JsonProperty("quote_id")]
        public int CoinmarketCapQuoteId { get; set; }

        /// <summary>
        /// Last price
        /// </summary>
        [JsonProperty("last_Price")]
        public decimal LastPrice { get; set; }

        /// <summary>
        /// Volume in base currency
        /// </summary>    
        [JsonProperty("base_volume")] 
        public decimal Volume { get; set; }

        /// <summary>
        /// Volume in quote currency
        /// </summary>        
        [JsonProperty("quote_volume")]
        public decimal QuoteVolume { get; set; }

        /// <summary>
        /// Identifies if trades are closed
        /// </summary>
        [JsonProperty("isFrozen")]
        public bool IsFrozen { get; set; }

        /// <summary>
        /// Change in percent between open and last prices
        /// </summary>        
        [JsonProperty("change")]
        public decimal Change { get; set; }
    }
}