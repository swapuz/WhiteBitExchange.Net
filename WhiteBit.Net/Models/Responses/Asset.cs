using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteBit.Net.Helpers;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitAsset : WhiteBitRawAsset
    {
        public WhiteBitAsset(WhiteBitRawAsset parent) : base(parent)
        {
        }

        /// <summary>
        /// Currency
        /// </summary>
        public string? Currency { get; set; }
    }

    public class WhiteBitRawAsset : ReflectableParent<WhiteBitRawAsset>
    {
        public WhiteBitRawAsset(WhiteBitRawAsset parent) : base(parent)
        {
        }

        [JsonProperty("name")]
        public string Name { get; set; } = string.Empty;

        [JsonProperty("unified_cryptoasset_id")]
        public long UnifiedCryptoassetId { get; set; }

        [JsonProperty("can_withdraw")]
        public bool CanWithdraw { get; set; }

        [JsonProperty("can_deposit")]
        public bool CanDeposit { get; set; }

        [JsonProperty("min_withdraw")]
        public decimal MinWithdraw { get; set; }

        [JsonProperty("max_withdraw")]
        public decimal MaxWithdraw { get; set; }

        [JsonProperty("maker_fee")]
        public decimal MakerFee { get; set; }

        [JsonProperty("taker_fee")]
        public decimal TakerFee { get; set; }

        [JsonProperty("min_deposit")]
        public decimal MinDeposit { get; set; }

        [JsonProperty("max_deposit")]
        public decimal MaxDeposit { get; set; }
        /// <summary>
        /// Currency networks. It might be a list of networks for cryptocurrency networks 
        /// or just a single item list for simple cryptocurrencies or tokens
        /// </summary>
        [JsonProperty("networks")]
        public WiteBitCryptoCurrNetworks? Networks { get; set; }
        public WiteBitFiatProviders? FiatProvider { get; set; }

        /// <summary>
        /// This object will be returned when currency has several networks/providers
        /// </summary>
        [JsonProperty("limits")]
        public WhitebitCurrencyLimits? Limits { get; set; }

        [JsonProperty("currency_precision")]
        public int CurrencyPrecision { get; set; }

        [JsonProperty("is_memo")]
        public bool IsMemo { get; set; }
    }
    public class WhitebitCurrencyLimits
    {
        /// <summary>
        /// Deposits limits
        /// </summary>
        [JsonProperty("deposit")]
        public Dictionary<string, WhitebitTransactionLimits>? Deposit { get; set; }

        [JsonProperty("withdraw")]
        public Dictionary<string, WhitebitTransactionLimits>? Withdraw { get; set; }
    }
    public class WhitebitTransactionLimits
    {
        /// <summary>
        /// Min deposit amount
        /// </summary>
        [JsonProperty("min")]
        public decimal? Min { get; set; }

        /// <summary>
        /// Max withdraw amount
        /// If there is no max limit, it is not returned
        /// </summary>
        [JsonProperty("max")]
        public decimal? Max { get; set; }
    }
    public class WiteBitCryptoCurrNetworks : WiteBitCurencyBaseProviders
    {
    }
    public class WiteBitFiatProviders : WiteBitCurencyBaseProviders
    {
    }
    public class WiteBitCurencyBaseProviders
    {
        /// <summary>
        /// Networks available for depositing
        /// </summary>
        [JsonProperty("deposits")]
        public List<string> Deposits { get; set; } = new List<string>();
        /// <summary>
        /// Networks available for withdrawing
        /// </summary>
        [JsonProperty("withdraws")]
        public List<string> Withdraws { get; set; } = new List<string>();

        /// <summary>
        /// Default network for depositing / withdrawing
        /// </summary>
        [JsonProperty("default")]
        public string Default { get; set; } = string.Empty;
    }
}