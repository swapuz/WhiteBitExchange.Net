using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Newtonsoft.Json.Linq;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net.Interfaces
{
    public interface IWhiteBitSocketClientSpotStreams
    {

        // Task<CallResult<UpdateSubscription>> SubscribeToOrderBookAsync(string symbol, Action<WhiteBitSocketResponse<>> dataHandler);
        // Task<CallResult<UpdateSubscription>> SubscribeToBalanceAsync(Action<WhiteBitSocketResponse<херйогознаЯкйогоописати>> dataHandler, params string[] assets);
        // Task<CallResult<UpdateSubscription>> SubscribeToCandlesAsync(HitBtcSubscribeToCandlesParam requestParams, Action<HitBtcSocketCandlesEvent> dataHandler);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataHandler">The handler of update data</param>
        /// <param name="symbols">place one or more symbols to subscribe</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToActiveOrders(Action<OrderSocketUpdate?>
         dataHandler, CancellationToken ct = default, params string[] symbols);

    }
}