using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitTradingBalance : WhiteBitRawTradingBalance
    {

        /// <summary>
        /// Currency
        /// </summary>
        public string? Currency { get; set; }
    }

    public class WhiteBitRawTradingBalance : IConvertible<WhiteBitTradingBalance>
    {

        /// <summary>
        /// Available balance of currency for trading
        /// </summary>
        [JsonProperty("available")]
        public decimal Available { get; set; }

        /// <summary>
        /// Balance of currency that is currently in active orders
        /// </summary>
        [JsonProperty("freeze")]
        public decimal Freeze { get; set; }
    }
}