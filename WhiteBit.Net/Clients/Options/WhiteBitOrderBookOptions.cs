using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Clients.Options
{
    public class WhiteBitOrderBookOptions : OrderBookOptions
    {
        public static WhiteBitOrderBookOptions Default { get; set; } = new WhiteBitOrderBookOptions();
        public WhiteBitOrderBookOptions(int maxEntriesAmount = 100, OrderBookSocketAggregationLevel aggregationLevel = OrderBookSocketAggregationLevel.NoAggregation)
        {
            MaxEntriesAmount = maxEntriesAmount;
            AggregationLevel = aggregationLevel;
        }
        public int MaxEntriesAmount;
        public OrderBookSocketAggregationLevel AggregationLevel { get; set; }

    }
}