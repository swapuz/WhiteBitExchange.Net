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
    public class WhiteBitMainBalance : WhiteBitRawMainBalance, IConvertible<Balance>
    {

        /// <summary>
        /// Currency
        /// </summary>
        public string? Currency { get; set; }


        #region IBaseRestClient
        private string? Asset => Currency;
        private decimal? Total => Available;
        #endregion
    }
    public class WhiteBitRawMainBalance : IConvertible<WhiteBitMainBalance>
    {

        /// <summary>
        /// Available balance of currency for transfer
        /// </summary>
        [JsonProperty("main_balance")]
        public decimal Available { get; set; }
    }
    public class WhiteBitTradingBalance : WhiteBitRawTradingBalance, IConvertible<Balance>
    {

        /// <summary>
        /// Currency
        /// </summary>
        public string? Currency { get; set; }


        #region IBaseRestClient
        private string? Asset => Currency;
        private decimal? Total => Available + Freeze;
        #endregion
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