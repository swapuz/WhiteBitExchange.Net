using System;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;

namespace WhiteBit.Net.Clients.Options
{
    public class WhiteBitSocketClientOptions : SocketApiOptions
    {
        private const string SocketEndpoint = "wss://api.whitebit.com/ws";

        public WhiteBitSocketClientOptions(SocketApiOptions? commonStreamsOptions = null) : base()
        {
            //TODO
            SocketApiOptions = commonStreamsOptions ?? new SocketApiOptions() { };
            SocketApiOptions.MaxSocketConnections = 25;
            SocketExchangeOptions = new SocketExchangeOptions() { };
        }

        /// <summary>
        /// Default options for the spot client
        /// </summary>
        public static WhiteBitSocketClientOptions Default { get; set; } = new WhiteBitSocketClientOptions();

        public SocketApiOptions SocketApiOptions { get; set; }
        public SocketExchangeOptions SocketExchangeOptions { get; set; }
        public TimeSpan TimeOut { get; set; } = TimeSpan.FromSeconds(100);
        public int Weight { get; set; } = 10;
    }
}