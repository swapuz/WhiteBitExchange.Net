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
    public interface IWhiteBitSocketClientSpotStream : IWhiteBitSocketClientCommonStream
    {

        /// <summary>
        /// Subscribe to receive updates in spot balances.
        /// </summary>
        /// <param name="dataHandler">The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradingBalance(Action<IEnumerable<WhiteBitTradingBalance>> dataHandler, CancellationToken ct = default, params string[] symbols);
    }
}