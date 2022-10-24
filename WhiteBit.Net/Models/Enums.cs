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
        Buy = 2,
        /// <summary>
        /// Sell
        /// </summary>
        [EnumMember(Value = "sell")]
        Sell = 1
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
    public enum TraderRole
    {
        Maker = 1,
        Taker = 2
    }
    public enum SocketErrorCode
    {
        InvalidArgument = 1,
        InternalError = 2,
        ServiceUnavailable = 3,
        MethodNotFound = 4,
        ServiceTimeout = 5
    }
    public enum SubscriptionStatus
    {
        [EnumMember(Value = "success")]
        Success,
        [EnumMember(Value = "failed")]
        Failed
    }

    public enum SocketMethod
    {
        [EnumMember(Value = "authorize")]
        Authorize,
        [EnumMember(Value = "balanceSpot_request")]
        BalanceSpotRequest,
        [EnumMember(Value = "balanceSpot_subscribe")]
        BalanceSpotSubscribe,
        [EnumMember(Value = "balanceSpot_update")]
        BalanceSpotUpdate,
        [EnumMember(Value = "balanceSpot_unsubscribe")]
        BalanceSpotUnsubscribe,
    }
}