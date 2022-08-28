using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Responses
{
    public class BaseResponse<Tresult>
    where Tresult : class
    {
        [JsonProperty("success")]
        public bool IsSuccess { get; set;}
        [JsonProperty("message")]
        public string Message { get; set; } = string.Empty;
        [JsonProperty("result")]
        public Tresult? Result { get; set; }
    }
}