using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Models;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net.Clients
{
    public abstract class WhiteBitSocketCommonClient : SocketApiClient
    {
        protected Log _log;
        protected WhiteBitSocketClient _whiteBitSocketClient;
        public WhiteBitSocketCommonClient(Log log, WhiteBitSocketClient whiteBitSocketClient, WhiteBitSocketClientOptions options) :
            base(options, options.SpotStreamsOptions)
        {
            this._log = log;
            this._whiteBitSocketClient = whiteBitSocketClient;
        }

        public async Task<CallResult<UpdateSubscription>> SubscribeToActiveOrders(Action<OrderSocketUpdate?> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.ActiveOrdersSubscribe, symbols.ToUpper()),
                true,
                dataHandler,
                ct
            );
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataHandler">>The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="filter">allowed only one of Limit (for all Limit types), Market (for all Market types) or Any</param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        public async Task<CallResult<UpdateSubscription>> SubscribeToExecutedOrders(Action<IEnumerable<WhiteBitOrder>?> dataHandler, CancellationToken ct = default, WhiteBitOrderType filter = WhiteBitOrderType.Any, params string[] symbols)
        {
            filter = (int)filter > 2 ? WhiteBitOrderType.Any : filter;
            return await SubscribeInternal(
                new WhiteBitSocketRequest<object>(SocketOutgoingMethod.ExecutedOrdersSubscribe, new object[] {symbols.ToUpper(), filter}),
                true,
                dataHandler,
                ct
            );
        }

        protected override AuthenticationProvider CreateAuthenticationProvider(ApiCredentials credentials)
            => new WhiteBitAuthenticationProvider(credentials);

        internal async Task<CallResult<UpdateSubscription>> SubscribeInternal<TRequest, TUpdate>(
            WhiteBitSocketRequest<TRequest> request,
            bool authenticate,
            Action<TUpdate?> onData,
            CancellationToken ct)
        {
            return await _whiteBitSocketClient.SubscribeInternal<TRequest, WhiteBiteSocketUpdateEvent<TUpdate>>(
                this,
                BaseAddress,
                request,
                authenticate,
                (dataWithServiceInfo) => onData(dataWithServiceInfo.Data.Data),
                ct);
        }
    }
}