using CryptoExchange.Net.Interfaces.CommonClients;
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
        /// <param name="requestWeight"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<WhiteBitTradingBalance>> GetBalanceAsync(string currency, int requestWeight = 1, CancellationToken ct = default);

        /// <summary>
        /// retrieves all balances.
        /// </summary>
        /// <param name="requestWeight"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        Task<WebCallResult<IEnumerable<WhiteBitTradingBalance>>> GetBalancesAsync(int requestWeight = 1, CancellationToken ct = default);

        Task<WebCallResult<Dictionary<string, WhiteBitTicker>>> GetTickersAsync(int requestWeight = 1, CancellationToken ct = default);


    }
}
