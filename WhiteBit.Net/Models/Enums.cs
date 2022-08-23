using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WhiteBit.Net.Models.Enums
{
    /// <summary>
    /// The side of an order
    /// </summary>
    public enum WhiteBitOrderSide
    {
        /// <summary>
        /// Buy
        /// </summary>
        [EnumMember(Value = "buy")]
        Buy,
        /// <summary>
        /// Sell
        /// </summary>
        [EnumMember(Value = "sell")]
        Sell
    }
    /// <summary>
    /// The type of the order
    /// </summary>
    public enum WhiteBitOrderType
    {
        [EnumMember(Value = "limit")]
        Limit,
        [EnumMember(Value = "market")]
        Market,
        [EnumMember(Value = "stop limit")]
        StopLimit,
        [EnumMember(Value = "stop market")]
        StopMarket,
        [EnumMember(Value = "stock market")]
        StockMarket
    }
    public enum WhiteBitProductType
    {
        Futures,
        Perpetual,
        Options

    }
}