using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Responses
{
    public class BaseSocketResponse<Tresult>
    {
        /// <summary>
        /// Id of request.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set;}
        /// <summary>
        /// Null for success
        /// </summary>
        [JsonProperty("error")]
        public WhiteBitSocketError? Error { get; set; }
        /// <summary>
        /// Null for failure
        /// </summary>
        [JsonProperty("result")]
        public Tresult? Result { get; set; }
    }

    public class SocketQueryResult
    {
        [JsonProperty("status")]
        public SubscriptionStatus? Status { get; set; }
    }

    internal class AuthorizeSocketResponse : BaseSocketResponse<SocketQueryResult>
    {
        
    }
}