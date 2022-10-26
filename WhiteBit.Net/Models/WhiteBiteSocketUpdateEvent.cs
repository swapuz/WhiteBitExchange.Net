using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiteBit.Net.Interfaces;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models
{
    public class WhiteBiteSocketUpdateEvent<TParam> : IWhiteBitSocketDataMethod<SocketIncomeMethod>
    {
        /// <summary>
        /// Id of request. 
        /// Use internal accessor to hide it from the enduser.
        /// </summary>
        [JsonProperty("id")]
        public int? Id { get; set; }

        /// <summary>
        /// Name of request.
        /// </summary>
        [JsonProperty("method")]

        public SocketIncomeMethod Method { get; set; }
        /// <summary>
        /// Here you pass params for method.
        /// </summary>
        [JsonProperty("params")]
        public IEnumerable<TParam> Parameters { get; set; } = Array.Empty<TParam>();
    }
}