using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitRestTicker : WhiteBitRawRestTicker
    {

        public string? Symbol { get; set; }

        internal Ticker ToCryptoExchangeTicker()
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
    public class WhiteBitRawRestTicker : IConvertible<WhiteBitRestTicker>
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

    public class WhiteBitTicker 
    {
        /// <summary>
        /// Symbol
        /// </summary>
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// open price that was at 'now - period' time
        /// </summary>
        [JsonProperty("open")]
        public decimal Open { get; set; }
        /// <summary>
        /// Last trade price
        /// </summary>
        [JsonProperty("last")]
        public decimal LastPrice { get; set; }
        /// <summary>
        /// lowest price
        /// </summary>
        [JsonProperty("low")]
        public decimal LowPrice { get; set; }
        /// <summary>
        /// highest price
        /// </summary>
        [JsonProperty("high")]
        public decimal HighPrice { get; set; }
        /// <summary>
        /// volume in stock
        /// </summary>
        [JsonProperty("volume")]
        public decimal BaseVolume { get; set; }
        /// <summary>
        /// volume in money
        /// </summary>
        [JsonProperty("deal")]
        public decimal QuoteVolume { get; set; }
    }
public class WhiteBitCustomPeriodTicker : WhiteBitTicker
{
        /// <summary>
        /// period
        /// </summary>
        [JsonProperty("period"), JsonConverter(typeof(SecondsToTimeSpanConverter))]
        public TimeSpan Period { get; set; }
        /// <summary>
        /// price that closes this period
        /// </summary>
        [JsonProperty("close")]
        public decimal Close { get; set; }
}

    [JsonConverter(typeof(ArrayConverter))]
    internal class WhiteBitTickerAsArray
    {
        [ArrayProperty(0)]
        public string? Symbol { get; set; }
        [ArrayProperty(1), JsonConverter(typeof(ObjectJsonConverter<WhiteBitTicker>))]
        public WhiteBitTicker? Body { get; set; }
    }
    
    [JsonConverter(typeof(ArrayConverter))]
    internal class WhiteBitCustomPeriodTickerAsArray
    {
        [ArrayProperty(0)]
        public string? Symbol { get; set; }
        [ArrayProperty(1), JsonConverter(typeof(ObjectJsonConverter<WhiteBitCustomPeriodTicker>))]
        public WhiteBitCustomPeriodTicker? Body { get; set; }
    }
}