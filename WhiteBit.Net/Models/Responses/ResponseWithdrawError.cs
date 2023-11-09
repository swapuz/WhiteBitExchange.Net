using System;
using Newtonsoft.Json;
namespace WhiteBit.Net.Models.Responses
{
	public class ResponseWithdrawError
	{
		[JsonProperty("code")]
		public int Code { get; set; }
		[JsonProperty("message")]
		public string Message { get; set; }
		[JsonProperty("errors")]
		public ErrorsModel Errors { get; set; }
		public class ErrorsModel
		{
			[JsonProperty("address")]
			public List<string> Address { get; set; }
			[JsonProperty("amount")]
			public List<string> Amount { get; set; }
			[JsonProperty("ticker")]
			public List<string> Ticker { get; set; }
			[JsonProperty("uniqueId")]
			public List<string> UniqueId { get; set; }
			[JsonProperty("memo")]
			public List<string> Memo { get; set; }


        }
	}
}

