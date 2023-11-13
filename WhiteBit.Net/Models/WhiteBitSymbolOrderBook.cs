using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.Sockets;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using WhiteBit.Net.Clients;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Responses;

namespace WhiteBit.Net.Models
{
    public class WhiteBitSpotSymbolOrderBook : WhiteBitCommonSymbolOrderBook
    {
        public WhiteBitSpotSymbolOrderBook(string symbol, WhiteBitOrderBookOptions? options = null, IWhiteBitSocketClientSpotStream? socketClientSteam = null) : base(symbol, options, socketClientSteam)
        {
        }

        protected override IWhiteBitSocketClientCommonStream ConstructNewSocketSteam()
        {
            return new WhiteBitSocketClient().SpotStreams;
        }
    }

    public class WhiteBitMarginSymbolOrderBook : WhiteBitCommonSymbolOrderBook
    {
        public WhiteBitMarginSymbolOrderBook(string symbol, WhiteBitOrderBookOptions? options = null, IWhiteBitSocketClientMarginStream? socketClientSteam = null) : base(symbol, options, socketClientSteam)
        {
        }

        protected override IWhiteBitSocketClientCommonStream ConstructNewSocketSteam()
        {
            return new WhiteBitSocketClient().MarginStreams;
        }
    }

    public abstract class WhiteBitCommonSymbolOrderBook : SymbolOrderBook
    {
        static long orderBookSequenceNumber = 0;
        int MaxEntriesAmount;
        OrderBookSocketAggregationLevel AggregationLevel;

        IWhiteBitSocketClientCommonStream _socketClient;

        public WhiteBitCommonSymbolOrderBook(string symbol, WhiteBitOrderBookOptions? options = null, IWhiteBitSocketClientCommonStream? socketClientSteam = null, ILogger<WhiteBitCommonSymbolOrderBook> logger = null)
            : base(logger, "WhiteBit", symbol)
        {
            var opt = options ?? WhiteBitOrderBookOptions.Default;
            _socketClient = socketClientSteam ?? ConstructNewSocketSteam();
            AggregationLevel = opt.AggregationLevel;
            MaxEntriesAmount = opt.MaxEntriesAmount;
        }
        /// <summary>
        /// should be something like return new WhiteBitSocketClient().[stream_type]Streams
        /// </summary>
        /// <returns></returns>
        protected abstract IWhiteBitSocketClientCommonStream ConstructNewSocketSteam();

        protected override async Task<CallResult<bool>> DoResyncAsync(CancellationToken ct)
        {
            return await WaitForSetOrderBookAsync(TimeSpan.FromSeconds(10.0), ct).ConfigureAwait(false);
        }

        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            return await _socketClient.SubscribeToOrderBook(OnUpdate, Symbol, MaxEntriesAmount, AggregationLevel, ct).ConfigureAwait(false);
        }

        private void OnUpdate(WhiteBitSocketOrderBook dataEvent)
        {
            if (dataEvent.IsFullReload)
            {
                SetInitialOrderBook(orderBookSequenceNumber++, dataEvent.Bids, dataEvent.Asks);
            }
            else
            {
                UpdateOrderBook(orderBookSequenceNumber++, dataEvent.Bids, dataEvent.Asks);
            }
        } 
    }
}