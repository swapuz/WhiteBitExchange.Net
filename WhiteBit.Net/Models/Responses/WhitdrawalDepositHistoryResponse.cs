using System;
using CryptoExchange.Net.Converters;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
	public class WhitdrawalDepositHistoryResponse
	{
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("offset")]
        public int Offset { get; set; }
        [JsonProperty("total")]
        public int Total { get; set; }
        [JsonProperty("records")]
        public List<RecordModel> Data { get; set; }
        public class RecordModel
        {
            [JsonProperty("address")]
            public string Address { get; set; }
            [JsonProperty("uniqueId")]
            public string UniqueId { get; set; }

            [JsonProperty("createdAt")]
            [JsonConverter(typeof(DateTimeConverter))]
            public DateTime CreatingAt { get; set; }
            [JsonProperty("currency")]
            public string Currency { get; set; }
            [JsonProperty("network")]
            public string Network { get; set; }
            [JsonProperty("ticker")]
            public string Token { get; set; }
            [JsonProperty("method")]
            public WhiteBitHistoryMode Mode { get; set; }
            [JsonProperty("amount")]
            public decimal Amount { get; set; }
            [JsonProperty("transactionHash")]
            public string Hash { get; set; }
            [JsonProperty("transactionId")]
            public string Id { get; set; }
            [JsonProperty("status")]
            public WhiteBitHistoryStatus Status { get; set; }
            [JsonProperty("memo")]
            public string Memo { get; set; }
            [JsonProperty("fee")]
            public decimal Fee { get; set; }
        }
    }
}

