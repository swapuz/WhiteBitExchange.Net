﻿using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
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
        Task<WebCallResult<WhiteBitTradingBalance?>> GetBalanceAsync(string currency, CancellationToken ct = default);

        /// <summary>
        /// retrieves all balances.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<WhiteBitTradingBalance>?>> GetBalancesAsync(CancellationToken ct = default);

        /// <summary>
        /// This method retrieves a 24-hour pricing and volume summary for each market pair available on the exchange.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        new Task<WebCallResult<IEnumerable<WhiteBitRestTicker>?>> GetTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// This method retrieves the assets status.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<WhiteBitAsset>?>> GetAssetsAsync(CancellationToken ct = default);
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

        /// <summary>
        /// This method retrieves the trades that have been executed recently on the requested market.
        /// </summary>
        /// <param name="symbol">trading pair</param>
        /// <param name="side">Can be buy or sell</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<WhiteBitPublicTrade>?>> GetPublicTradesAsync(string symbol, WhiteBitOrderSide? side = null, CancellationToken ct = default);
        /// <summary>
        /// This method returns the list of available futures markets.
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<List<WhiteBitFutures>?>> GetFuturesAsync(CancellationToken ct = default);
        /// <summary>
        /// This method returns the list of markets that available for collateral trading
        /// </summary>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<List<string>>> GetCollateralMarketsAsync(CancellationToken ct = default);
        /// <summary>
        /// place order
        /// </summary>
        /// <param name="parameters">create the instance with one of static methods of WhiteBitPlaceOrderRequest class
        /// e.g. `WhiteBitPlaceOrderRequest.CreateLimitOrderRequest()`</param>
        /// <param name="ct"></param>
        /// <returns>order model</returns>
        Task<WebCallResult<WhiteBitOrder>> PlaceOrderAsync(WhiteBitPlaceOrderRequest parameters, CancellationToken ct = default);

        /// <summary>
        /// Cancel existing order
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="orderId">Order Id. Example: 4180284841</param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<WhiteBitOrder>> CancelOrderAsync(string symbol, long orderId, CancellationToken ct = default);
        /// <summary>
        /// This method retrieves unexecuted orders only.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<List<WhiteBitOrder>>> GetActiveOrdersAsync(GetActiveOrdersRequest request, CancellationToken ct = default);
        /// <summary>
        /// This method retrieves executed order histExecutby market.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<Dictionary<string, IEnumerable<WhiteBitOrder>>>> GetExecutedOrdersAsync(GetExecutedOrdersRequest? request = null, CancellationToken ct = default);
        Task<WebCallResult<Dictionary<string, IEnumerable<WhiteBitUserTrade>>>> GetOwnTradesAsync(GetOwnTradesRequest? request = null, CancellationToken ct = default);
        /// <summary>
        /// This method retrieves deals history details on pending or executed order.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="ct"></param>
        /// <returns></returns>This endpoint retrieves deals history details on pending or executed order.
        Task<WebCallResult<IEnumerable<WhiteBitUserTrade>?>> GetOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct = default);
        /// <summary>
        /// Generate new Address
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="network"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<GenerateNewAddress>> GenerateNewAddress(string symbol, string network = null, CancellationToken ct = default);
        /// <summary>
        /// Get Deposite Adress
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="network"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<GenerateNewAddress>> GetDepositeAddress(string symbol, string network = null, CancellationToken ct = default);
        /// <summary>
        /// Get Deposite and witdrawal history
        /// </summary>
        /// <param name="model"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<WhitdrawalDepositHistoryResponse>> GetDepositeWithdrawalHistory(WhitdrawalDepositHistoryRequest model, CancellationToken ct = default);
        /// <summary>
        ///  Wihthdrawal requst
        /// </summary>
        /// <param name="syymbol"></param>
        /// <param name="amount"></param>
        /// <param name="address"></param>
        /// <param name="uniqueId"></param>
        /// <param name="memo"></param>
        /// <param name="network"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<ResponseWithdrawError>> WithdrawRequest(string syymbol, decimal amount, string address, string uniqueId, string memo = "", string network = "", bool isFee = false, CancellationToken ct = default);
        /// <summary>
        /// transfer with account
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="token"></param>
        /// <param name="amount"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<ResponseTransferAmountError>> TransferAmount(string from, string to, string token, decimal amount, CancellationToken ct = default);
    }
}
