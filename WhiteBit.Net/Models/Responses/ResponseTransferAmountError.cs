using System;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Responses
{
	public class ResponseTransferAmountError
	{

        public int Code { get; set; }
        [JsonProperty("message")]
        public string Message { get; set; }
        [JsonProperty("errors")]
        public ErrorsModel Errors { get; set; }
        public class ErrorsModel
        {
            [JsonProperty("method")]
            public List<string> Method { get; set; }
            [JsonProperty("amount")]
            public List<string> Amount { get; set; }
            [JsonProperty("ticker")]
            public List<string> Ticker { get; set; }
            
        }
    }
}

