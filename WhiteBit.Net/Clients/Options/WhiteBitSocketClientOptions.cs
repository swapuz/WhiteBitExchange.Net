using System;
using CryptoExchange.Net.Objects;

namespace WhiteBit.Net.Clients.Options
{
    public class WhiteBitSocketClientOptions : ClientOptions
    {
        private const string SocketEndpoint = "wss://api.whitebit.com/ws";

        public WhiteBitSocketClientOptions(SocketApiClientOptions? commonStreamsOptions = null) : base()
        {
            CommonStreamsOptions = commonStreamsOptions ?? new SocketApiClientOptions(SocketEndpoint);
            CommonStreamsOptions.SocketSubscriptionsCombineTarget = 25;
        }

        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static WhiteBitSocketClientOptions Default { get; set; } = new WhiteBitSocketClientOptions();

        public SocketApiClientOptions CommonStreamsOptions { get; set; }

    }
}