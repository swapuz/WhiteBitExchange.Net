using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Requests
{
    public class BaseSocketRequest<TParam>
    {
        /// <summary>
        /// Id of request.
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
        /// <summary>
        /// Name of request.
        /// </summary>
        [JsonProperty("method")]
        public SocketMethod Method { get; set; }
        /// <summary>
        /// Here you pass params for method.
        /// </summary>
        [JsonProperty("params")]
        public IEnumerable<TParam> Parameters { get; set; } = Array.Empty<TParam>();
    }

    internal class AuthorizeSocketRequest : BaseSocketRequest<string>
    {
        public AuthorizeSocketRequest(string token, int id = 0)
        {
            Id = id;
            Method = SocketMethod.Authorize;
            Parameters = new string[] { token, "public" }; // public is hardcoded 
        }
    }
    
    }