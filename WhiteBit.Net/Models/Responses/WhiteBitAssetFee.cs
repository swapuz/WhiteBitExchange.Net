using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitAssetFee : WhiteBitRawAssetFee
    {
       // public string? Currency { get; set; }
        public string? Network { get; set; }
    }

    public class WhiteBitRawAssetFee : IConvertible<WhiteBitAssetFee>
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("ticker")]
        public string Ticker { get; set; }
        [JsonProperty("is_depositable")]
        public bool? IsDepositable { get; set; }
        [JsonProperty("is_withdrawal")]
        public bool? IsWithdrawal { get; set; }
        [JsonProperty("is_api_depositable")]
        public bool? IsApiDepositable { get; set; }
        [JsonProperty("is_api_withdrawal")]
        public bool? IsApiWithdrawable { get; set; }
        [JsonProperty("deposit")]
        public WhiteBitAssetFeeITemModel Deposit { get; set; }
        [JsonProperty("withdraw")]
        public WhiteBitAssetFeeITemModel Withdrawal { get; set; }

    }

    public class WhiteBitAssetFeeITemModel
    {
        [JsonProperty("fixed")]
        
        public decimal? Fixed { get; set; }
        [JsonProperty("min_amount")]
        public decimal MinAmount { get; set; }
        [JsonProperty("max_amount")]
        public decimal MaxAmount { get; set; }
    }
}
