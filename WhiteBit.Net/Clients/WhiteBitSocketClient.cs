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
            SpotStreams = AddApiClient(new WhiteBitSocketClientSpotStream(log, this, options));
            MarginStreams = AddApiClient(new WhiteBitSocketClientMarginStream(log, this, options));
        }
        #endregion 
        public IWhiteBitSocketClientSpotStream SpotStreams { get; set; }
        public IWhiteBitSocketClientMarginStream MarginStreams { get; set; }

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
                        WebsocketToken = await ((WhiteBitApiClientV4)client.ApiClient).GetWebsocketToken();
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
                            NextId(),
                            (await GetWebsocketToken(socketConnection.ApiClient.AuthenticationProvider))!),
                            ClientOptions.SocketResponseTimeout,
                            data =>
                            {
                                AuthorizeSocketResponse? result = null;
                                try
                                {
                                    result = data.ToObject<AuthorizeSocketResponse>();
                                }
                                catch (Exception e)
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
            if (!IsResponseMatchesToRequest(request, data))
            {
                callResult = null;
                return false;
            }
            var response = data.ToObject<WhiteBitSocketResponse<T>>();
            callResult = (response is null || response.Result == null) ?
                                    new CallResult<T>(new ServerError(response!.Error?.ToString() ?? "Empty data came"))
                                    : new CallResult<T>(response.Result);
            return true;
        }

        protected override bool HandleSubscriptionResponse(SocketConnection socketConnection, SocketSubscription subscription, object request, JToken data, out CallResult<object>? callResult)
        {
            if (!IsResponseMatchesToRequest(request, data))
            {
                callResult = null;
                return false;
            }
            log.Write(LogLevel.Trace, $"Socket {socketConnection.SocketId} Subscription completed");
            callResult = new CallResult<object>(new object());
            return true;
        }

        private bool IsResponseMatchesToRequest(object request, JToken data)
        {
            if (data.Type != JTokenType.Object)
                return false;

            var respId = (int?)data["id"];
            if (respId == null || respId != ((IWhiteBitSocketDataMethod<SocketOutgoingMethod>)request).Id)
                return false;

            return true;
        }

        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, object request)
        {
            var methodOut = ((IWhiteBitSocketDataMethod<SocketOutgoingMethod>)request).Method;
            return methodOut.DoesMethodMatch(message["method"]?.ToObject<SocketIncomeMethod>()) == true;
        }

        protected override bool MessageMatchesHandler(SocketConnection socketConnection, JToken message, string identifier)
        {
            return true;
        }

        protected override async Task<bool> UnsubscribeAsync(SocketConnection connection, SocketSubscription subscriptionToUnsub)
        {
            var unSubReq = ((WhiteBitSocketRequest<object>)subscriptionToUnsub.Request!).Method.GetCorrespondingUnsubscribeMethod();
            if (unSubReq is null)
                return false;
            var request = new UnsubscribeRequest(NextId(), unSubReq.Value);
            var result = await QueryAndWaitAsync<SocketStatusResult>(connection, request);
            return result.Data?.Status == SubscriptionStatus.Success;
        }

        internal async Task<CallResult<UpdateSubscription>> SubscribeInternal<TRequest, TUpdate>
            (SocketApiClient apiClient,
            string url,
            WhiteBitSocketRequest<TRequest> request,
            bool authenticate,
            Action<DataEvent<TUpdate>> onData,
            CancellationToken ct)
        {
            request.Id = NextId();
            return await SubscribeAsync(apiClient, url, request, null, authenticate, onData, ct);
        }
    }
}