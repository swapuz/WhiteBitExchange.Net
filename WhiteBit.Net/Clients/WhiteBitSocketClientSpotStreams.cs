using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
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
    public class WhiteBitSocketClientSpotStream : WhiteBitSocketCommonClient, IWhiteBitSocketClientSpotStream
    {
        internal WhiteBitSocketClientSpotStream(Log log, WhiteBitSocketClient whiteBitSocketClient, WhiteBitSocketClientOptions options) :
            base(log, whiteBitSocketClient, options)
        {
        }
        ///<inheritdoc/>
        public async Task<CallResult<UpdateSubscription>> SubscribeToTradingBalance(Action<IEnumerable<WhiteBitTradingBalance>> dataHandler, CancellationToken ct = default, params string[] symbols)
        {
            return await SubscribeInternal<string, IEnumerable<Dictionary<string, WhiteBitRawTradingBalance>>>(
                new WhiteBitSocketRequest<string>(SocketOutgoingMethod.BalanceSpotSubscribe, symbols.ToUpper()),
                true,
                balAsDict => { dataHandler(balAsDict!.SelectMany(x => x).Select(b => b.Value.Convert(new WhiteBitTradingBalance { Currency = b.Key })!)); },
                ct
            );
        }
    }
}