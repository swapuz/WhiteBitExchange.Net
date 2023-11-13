using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Helpers;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Requests;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net.Clients
{
    public class WhiteBitSocketClientMarginStream : WhiteBitSocketCommonClient, IWhiteBitSocketClientMarginStream
    {
        internal WhiteBitSocketClientMarginStream(ILogger Logger, WhiteBitSocketClient whiteBitSocketClient, WhiteBitSocketClientOptions options) :
            base(Logger, whiteBitSocketClient, options)
        {
        }
        ///<inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradingBalance(Action<Dictionary<string, decimal>> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal<string, IEnumerable<Dictionary<string, decimal>>>(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.BalanceMarginSubscribe, symbols.ToUpper()),
                true,
                balAsArray => { dataHandler(balAsArray!.SelectMany(x => x).ToDictionary(x => x.Key, x=> x.Value)); },
                ct
            );
        }
    }
}