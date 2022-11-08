using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Responses
{
    [JsonConverter(typeof(ArrayConverter))]
    public class LastPrice
    {
        [ArrayProperty(0)]
        public string Symbol { get; set; } = string.Empty;
        [ArrayProperty(1)]
        public decimal Price {get; set; }
    }
}