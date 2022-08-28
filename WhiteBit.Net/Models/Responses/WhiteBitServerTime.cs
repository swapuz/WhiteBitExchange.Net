using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitServerTime
    {
        [JsonProperty("time"), JsonConverter(typeof(DateTimeConverter))]
        public DateTime Time { get; set; }
    }
}