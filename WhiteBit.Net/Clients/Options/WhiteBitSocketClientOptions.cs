using System;
using CryptoExchange.Net.Objects;

namespace WhiteBit.Net.Clients.Options
{
    public class WhiteBitSocketClientOptions : BaseSocketClientOptions
    {
        private const string SocketEndpoint = "wss://api.whitebit.com/ws";

        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static WhiteBitSocketClientOptions Default { get; set; } = new WhiteBitSocketClientOptions()
        {
            SocketSubscriptionsCombineTarget = 10
        };

        internal ApiClientOptions SpotStreamsOptions { get; set; } = new ApiClientOptions(SocketEndpoint);

    }
}