using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net
{
    internal class WhiteBitSocketClientSpotStreams : SocketApiClient, IWhiteBitSocketClientSpotStreams
    {
        private Log _log;
        private WhiteBitSocketClient _whiteBitSocketClient;
        private WhiteBitSocketClientOptions _options;

        public WhiteBitSocketClientSpotStreams(Log log, WhiteBitSocketClient whiteBitSocketClient, WhiteBitSocketClientOptions options) :
            base(options, options.SpotStreamsOptions)
        {
            this._log = log;
            this._whiteBitSocketClient = whiteBitSocketClient;
            this._options = options;
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials) 
            => new WhiteBitAuthenticationProvider(credentials);
    }
}