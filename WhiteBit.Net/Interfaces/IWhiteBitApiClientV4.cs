﻿using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net.Interfaces
{
    public interface IWhiteBitApiClientV4:ISpotClient
    {
        /// <summary>
        /// retrieves the trade balance by currency ticker.
        /// </summary>
        /// <param name="currency">Currency's ticker. Example: BTC</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<WhiteBitTradingBalance>> GetBalanceAsync(string currency, CancellationToken ct = default);

        /// <summary>
        /// retrieves all balances.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<WhiteBitTradingBalance>>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// This method retrieves a 24-hour pricing and volume summary for each market pair available on the exchange.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        new Task<WebCallResult<IEnumerable<WhiteBitTicker>>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// This method retrieves the assets status.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<WhiteBitAsset>>> GetAssetsAsync(CancellationToken ct = default);
        /// <summary>
        /// This method retrieves the current order book as two arrays (bids / asks) with additional parameters.
        /// </summary>
        /// <param name="symbol">trading pair</param>
        /// <param name="depthLimit"> Orders depth quantity. Not defined or 0 will return full order book</param>
        /// <param name="AggregationLevel">Optional parameter that allows API user to see different level of aggregation. 
        /// Level 0 – default level, no aggregation. Starting from level 1 (lowest possible aggregation) 
        /// and up to level 5 - different levels of aggregated orderbook.</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<WhiteBitOrderBook>> GetOrderBookAsync(string symbol, int? depthLimit = null, int? AggregationLevel = null, CancellationToken ct = default);
    }
}
