using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Requests
{
    public class AccountBalanceSummaryRequest
    {
        /*{
  "ticker": "BTC",
  "request": "{{request}}",
  "nonce": "{{nonce}}"
}*/
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("request")]
        public string Request { get; set; }
        [JsonProperty("nonce")]
        public string Nonce { get; set; }
    }
}