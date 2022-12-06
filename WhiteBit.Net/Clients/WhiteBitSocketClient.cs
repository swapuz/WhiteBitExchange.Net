using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Models;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;
using Microsoft.Extensions.Logging;
using WhiteBit.Net.Clients.Options;

namespace WhiteBit.Net.Clients
{
    public class WhiteBitSocketClient : BaseSocketClient, IWhiteBitSocketClient
    {
        #region constructor/destructor

        /// <summary>
        /// Create a new instance of BinanceSocketClientSpot with default options
        /// </summary>
        public WhiteBitSocketClient() : this(WhiteBitSocketClientOptions.Default)
        {
        }

        /// <summary>
        /// Create a new instance of BinanceSocketClientSpot using provided options
        /// </summary>
        /// <param name="options">The options to use for this client</param>
        public WhiteBitSocketClient(WhiteBitSocketClientOptions options) : base("WhiteBit", options)
        {
            // RateLimitPerSocketPerSecond = 4;
            SpotStreams = AddApiClient(new WhiteBitSocketClientSpotStream(log, this, options));
            MarginStreams = AddApiClient(new WhiteBitSocketClientMarginStream(log, this, options));
        }
        #endregion 
        public IWhiteBitSocketClientSpotStream SpotStreams { get; set; }
        public IWhiteBitSocketClientMarginStream MarginStreams { get; set; }

    }
}