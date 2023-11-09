using System;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Requests
{
	public class WhitdrawalDepositHistoryRequest
	{
		[JsonProperty("transactionMethod")]
		public WhiteBitHistoryMode TransactionMethod { get; set; }
		[JsonProperty("ticker")]
		public string Token { get; set; }
		[JsonProperty("address")]
		public string Address { get; set; }
		[JsonProperty("addresses")]
		public List<string> Addresses { get; set; }
		[JsonProperty("uniqueId")]
		public string UniqueId { get; set; }
		[JsonProperty("limit")]
		public int? Limit { get; set; }
		[JsonProperty("offset")]
		public int? Offset { get; set; }
		[JsonProperty("status")]
		public List<WhiteBitHistoryStatus> Status { get; set; }
    }
}

