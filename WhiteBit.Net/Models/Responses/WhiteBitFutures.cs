using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitFutures
    {
        [JsonProperty("ticker_id")]
        public string TickerId { get; set; } = string.Empty;

        [JsonProperty("stock_currency")]
        public string StockCurrency { get; set; } = string.Empty;

        [JsonProperty("money_currency")]
        public string MoneyCurrency { get; set; } = string.Empty;

        [JsonProperty("last_price")]
        public decimal LastPrice { get; set; }

        [JsonProperty("stock_volume")]
        public decimal StockVolume { get; set; }

        [JsonProperty("money_volume")]
        public decimal MoneyVolume { get; set; }

        [JsonProperty("bid")]
        public decimal Bid { get; set; }

        [JsonProperty("ask")]
        public decimal Ask { get; set; }

        [JsonProperty("high")]
        public decimal High { get; set; }

        [JsonProperty("low")]
        public decimal Low { get; set; }

        [JsonProperty("product_type")]
        public WhiteBitProductType ProductType { get; set; }

        [JsonProperty("open_interest")]
        public long OpenInterest { get; set; }

        [JsonProperty("index_price")]
        public decimal IndexPrice { get; set; }

        [JsonProperty("index_name")]
        public string IndexName { get; set; } = string.Empty;

        [JsonProperty("index_currency")]
        public string IndexCurrency { get; set; } = string.Empty;

        [JsonProperty("funding_rate")]
        public decimal FundingRate { get; set; }

        [JsonProperty("next_funding_rate_timestamp")]
        [JsonConverter(typeof(DateTimeConverter))]
        public DateTime NextFundingRateTimestamp { get; set; }
    }
}