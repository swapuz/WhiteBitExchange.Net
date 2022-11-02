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
    public interface IWhiteBitSocketClientCommonStream
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataHandler">The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="symbols">place one or more symbols to subscribe</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToActiveOrders(Action<OrderSocketUpdate?>
         dataHandler, CancellationToken ct = default, params string[] symbols);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataHandler">>The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="filter">allowed only one of Limit (for all Limit types), Market (for all Market types) or Any</param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToExecutedOrders(Action<IEnumerable<WhiteBitOrder>?> dataHandler, CancellationToken ct = default, WhiteBitOrderType filter = WhiteBitOrderType.Any, params string[] symbols);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataHandler">The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="symbols">place one or more symbols to subscribe</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUserTrades(Action<WhiteBitUserTrade> dataHandler, CancellationToken ct = default, params string[] symbols);
        /// <summary>
        /// The requested interval must meet the following conditions:
        /// If the number is less than 60, then 60 must be divisible by the requested number without a remainder;
        /// Less than 3600 (1 hour) - the number must be divisible by 60 without a remainder, and 3600 must be divisible by the requested number without a remainder;
        /// Less than 86400 (day) - the number must be whitened by 3600 without a remainder, and 86400 must be divisible by the number without a remainder;
        /// Less than 86400 * 7 (week) - the number must be divisible by 86400 without a remainder;
        /// Equal to 86400 * 7;
        /// Equal to 86400 * 30.
        /// </summary>
        /// <param name="dataHandler">The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="symbols">place one or more symbols to subscribe</param>
        /// <param name="intervalInSeconds">
        /// The requested interval must meet the following conditions:
        /// If the number is less than 60, then 60 must be divisible by the requested number without a remainder;
        /// Less than 3600 (1 hour) - the number must be divisible by 60 without a remainder, and 3600 must be divisible by the requested number without a remainder;
        /// Less than 86400 (day) - the number must be whitened by 3600 without a remainder, and 86400 must be divisible by the number without a remainder;
        /// Less than 86400 * 7 (week) - the number must be divisible by 86400 without a remainder;
        /// Equal to 86400 * 7;
        /// Equal to 86400 * 30.</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToCurrentCandles(Action<IEnumerable<WhiteBitCandle>> dataHandler, string symbol, int intervalInSeconds, CancellationToken ct = default);
        /// <summary>
        /// The requested interval must meet the following conditions:
        /// If the number is less than 60, then 60 must be divisible by the requested number without a remainder;
        /// Less than 3600 (1 hour) - the number must be divisible by 60 without a remainder, and 3600 must be divisible by the requested number without a remainder;
        /// Less than 86400 (day) - the number must be whitened by 3600 without a remainder, and 86400 must be divisible by the number without a remainder;
        /// Less than 86400 * 7 (week) - the number must be divisible by 86400 without a remainder;
        /// Equal to 86400 * 7;
        /// Equal to 86400 * 30.
        /// </summary>
        /// <param name="dataHandler">The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="symbols">place one or more symbols to subscribe</param>
        /// <param name="intervalInSeconds">
        /// The requested interval must meet the following conditions:
        /// If the number is less than 60, then 60 must be divisible by the requested number without a remainder;
        /// Less than 3600 (1 hour) - the number must be divisible by 60 without a remainder, and 3600 must be divisible by the requested number without a remainder;
        /// Less than 86400 (day) - the number must be whitened by 3600 without a remainder, and 86400 must be divisible by the number without a remainder;
        /// Less than 86400 * 7 (week) - the number must be divisible by 86400 without a remainder;
        /// Equal to 86400 * 7;
        /// Equal to 86400 * 30.</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToRecentlyClosedCandles(Action<WhiteBitCandle> dataHandler, string symbol, int intervalInSeconds, CancellationToken ct = default);
        /// <summary>
        /// Market statistics for current day UTC
        /// </summary>
        /// <param name="dataHandler">The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="symbols">place one or more symbols to subscribe</param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToUtcDayStartTicker(Action<WhiteBitTicker> dataHandler, CancellationToken ct = default, params string[] symbols);
        Task<CallResult<UpdateSubscription>> SubscribeTo24HAgoTicker(Action<WhiteBitCustomPeriodTicker> dataHandler, CancellationToken ct = default, params string[] symbols);

    }
}