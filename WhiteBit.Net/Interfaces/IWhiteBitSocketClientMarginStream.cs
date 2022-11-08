using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;

namespace WhiteBit.Net.Interfaces
{
    public interface IWhiteBitSocketClientMarginStream : IWhiteBitSocketClientCommonStream
    {
        /// <summary>
        /// Subscribe to receive updates in margin balances.
        /// Balance available for margin trade is equal to balance * leverage and it depends on liquidity in orderbook and your open positions. 
        /// When you open position, your balance will not change, but amount available for trade will decrease
        /// </summary>
        /// <param name="dataHandler">The handler of update data</param>
        /// <param name="ct">Cancellation token for closing this subscription</param>
        /// <param name="symbols"></param>
        /// <returns></returns>
        Task<CallResult<UpdateSubscription>> SubscribeToTradingBalance(Action<Dictionary<string, decimal>> dataHandler, CancellationToken ct = default, params string[] symbols);
    }
}