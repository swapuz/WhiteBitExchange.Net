using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net.Clients
{
    internal class WhiteBitSocketClientSpotStreams : SocketApiClient, IWhiteBitSocketClientSpotStreams
    {
        private Log _log;
        private WhiteBitSocketClient _whiteBitSocketClient;
        private WhiteBitSocketClientOptions _options;
        private WhiteBitSocketCommonClient _commonClient;
        public WhiteBitSocketClientSpotStreams(Log log, WhiteBitSocketClient whiteBitSocketClient, WhiteBitSocketClientOptions options) :
            base(options, options.SpotStreamsOptions)
        {
            this._log = log;
            this._whiteBitSocketClient = whiteBitSocketClient;
            this._options = options;
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeToActiveOrders(Action<OrderSocketUpdate?> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.ActiveOrdersSubscribe, symbols),
                true,
                dataHandler,
                ct
            );
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials) 
            => new WhiteBitAuthenticationProvider(credentials);

        private async Task<CallResult<UpdateSubscription>> SubscribeInternal<TRequest, TUpdate>(WhiteBitSocketRequest<TRequest> request, bool authenticate, Action<TUpdate?> onData, CancellationToken ct)
        {
            return await _whiteBitSocketClient.SubscribeInternal(this, BaseAddress, request, authenticate, onData, ct);
        }
    }
}