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
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net
{
    public class WhiteBitSocketClient : BaseSocketClient, IWhiteBitSocketClient
    {
        private static string? WebsocketToken;
        private static SemaphoreSlim WebSocketTokenSemaphore = new SemaphoreSlim(1, 1);
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
            SetDataInterpreter((data) => string.Empty, null);
            // RateLimitPerSocketPerSecond = 4;
            SpotStreams = AddApiClient(new WhiteBitSocketClientSpotStreams(log, this, options));
        }
        #endregion 
        public IWhiteBitSocketClientSpotStreams SpotStreams { get; set; }

        private static async Task<string?> GetWebsocketToken(AuthenticationProvider authProvider)
        {
            try
            {
                await WebSocketTokenSemaphore.WaitAsync();
                if (WebsocketToken is null)
                {
                    using (var client = new WhiteBitClient(
                        new WhiteBitClientOptions()
                            {
                                ApiCredentials = authProvider.Credentials
                            })
                    )
                    {
                        WebsocketToken = await ((WhiteBitApiClientV4) client.ApiClient).GetWebsocketToken();
                    }
                }
                return WebsocketToken;
            }
            finally
            {
                WebSocketTokenSemaphore.Release();
            }
        }
        protected override async Task<CallResult<bool>> AuthenticateSocketAsync(SocketConnection socketConnection)
        {
            // should I check this condition
            if (socketConnection.ApiClient.AuthenticationProvider is null)
                return new CallResult<bool>(false);
            bool isSuccess = false;
            ServerError? serverError = null;
            await socketConnection.SendAndWaitAsync(
                    new AuthorizeSocketRequest(
                            (await GetWebsocketToken(socketConnection.ApiClient.AuthenticationProvider))!),
                            ClientOptions.SocketResponseTimeout,
                            data =>
            {
                AuthorizeSocketResponse? result = null;
                try
                {
                    result = data.ToObject<AuthorizeSocketResponse>();
                }
                catch(Exception e)
                {
                    serverError = new ServerError($"Error while parse into AuthorizeSocketResponse: {e.Message}");
                return false;
                }

                if (data.ToObject<AuthorizeSocketResponse>()?.Result?.Status == SubscriptionStatus.Success)
                {
                    isSuccess = true;
                }
                else
                {
                    serverError = new ServerError($"Auth request was not successful: {result!.Error?.ToString()}");
                }
                return true;
            });
            return isSuccess ? new CallResult<bool>(true) : new CallResult<bool>(serverError!);
        }

        protected override bool HandleQueryResponse<T>(SocketConnection socketConnection, object request, JToken data, [NotNullWhen(true)] out CallResult<T>? callResult)
        {
            throw new NotImplementedException();
        }

        protected override bool HandleSubscriptionResponse(SocketConnection socketConnection, SocketSubscription subscription, object request, JToken data, out CallResult<object>? callResult)
        {
            throw new NotImplementedException();
        }

        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            throw new NotImplementedException();
        }

        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            throw new NotImplementedException();
        }

        protected override Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscriptionToUnsub)
        {
            throw new NotImplementedException();
        }
    }
}