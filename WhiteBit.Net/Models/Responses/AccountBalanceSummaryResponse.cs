using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Responses
{

    public class AccountBalanceSummaryItemResponse
    {
        /*[
      {
        "asset": "BTC",
        "balance": "0",
        "borrow": "0",
        "availableWithoutBorrow": "0",
        "availableWithBorrow": "123.456"
      }
    ]*/
        [JsonProperty("asset")]
        public string Asset { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("borrow")]
        public decimal Borrow { get; set; }
        [JsonProperty("availableWithoutBorrow")]
        public decimal AvailableWithoutBorrow { get; set; }
        [JsonProperty("availableWithBorrow")]
        public decimal AvailableWithBorrow { get; set; }
    }
}