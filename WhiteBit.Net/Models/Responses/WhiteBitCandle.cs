using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Responses
{
    [JsonConverter(typeof(ArrayConverter))]
    public class WhiteBitCandle
    {
        [ArrayProperty(0), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Timestamp { get;  set; }
        [ArrayProperty(1)]
        public decimal Open { get; set; }
        [ArrayProperty(2)]
        public decimal Close { get; set; }
        [ArrayProperty(3)]
        public decimal High { get; set; }
        [ArrayProperty(4)]
        public decimal Low { get; set; }
        [ArrayProperty(5)]
        public decimal BaseVolume { get; set; }
        [ArrayProperty(6)]
        public decimal QuoteVolume { get; set; }
        [ArrayProperty(7)]
        public string Symbol { get; set; } = string.Empty;
    }
}