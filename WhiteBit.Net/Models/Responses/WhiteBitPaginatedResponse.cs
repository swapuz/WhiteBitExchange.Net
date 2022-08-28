using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Responses
{
    public class WhiteBitPaginatedResponse<Tresult>
    where Tresult : class
    {
        [JsonProperty("offset")]
        public int Offset { get; set;}
        [JsonProperty("limit")]
        public int Limit { get; set; }
        [JsonProperty("records")]
        public Tresult? Result { get; set; }
    }
}