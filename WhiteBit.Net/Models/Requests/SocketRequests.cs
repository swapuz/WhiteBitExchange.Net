using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Requests
{
    public class WhiteBitSocketRequest<TParam>
    {
        /// <summary>
        /// Id of request. 
        /// Use internal accessor to hide it from the enduser.
        /// </summary>
        [JsonProperty("id")]
        internal int Id { get; set; }
        /// <summary>
        /// Name of request.
        /// </summary>
        [JsonProperty("method")]
        public SocketOutgoingMethod Method { get; set; }
        /// <summary>
        /// Here you pass params for method.
        /// </summary>
        [JsonProperty("params")]
        public IEnumerable<TParam> Parameters { get; set; } = Array.Empty<TParam>();
    }

    internal class AuthorizeSocketRequest : WhiteBitSocketRequest<string>
    {
        public AuthorizeSocketRequest(int id, string token)
        {
            Id = id;
            Method = SocketOutgoingMethod.Authorize;
            Parameters = new string[] { token, "public" }; // public is hardcoded 
        }
    }

    internal class UnsubscribeRequest : WhiteBitSocketRequest<string>
    {
        public UnsubscribeRequest(int id, SocketOutgoingMethod method)
        {
            Id = id;
            Method = method;
        }
    }

}