using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        /// <summary>
        /// for filtering only
        /// </summary>
        Any = 0,
        [EnumMember(Value = "limit")]
        Limit = 1,
        [EnumMember(Value = "market")]
        Market = 2,
        [EnumMember(Value = "stop limit")]
        StopLimit = 3,
        [EnumMember(Value = "stop market")]
        StopMarket = 4,
        ConditionalLimit = 5,
        ConditionalMarket = 6,
        MarginLimit = 7,
        MarginMarket = 8,
        MarginTriggerStopLimit = 9,
        MarginTriggerStopMarket = 10,
        [EnumMember(Value = "stock market")]
        StockMarket
    }
    
    /// <summary>
    /// If new order instantly matches an order from orderbook,
    /// then you will receive only one message with update event ID equal to 3.
    /// </summary>
    public enum SocketOrderUpdateEventType
    {
        NewOrder = 1,
        UpdateOrder = 2,
        /// <summary>
        /// Finish order (cancel or execute)
        /// </summary>
        FinishOrder = 3
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

    [JsonConverter(typeof(StringEnumConverter))]
    public enum SocketOutgoingMethod
    {
        [EnumMember(Value = "authorize")]
        Authorize,

        [EnumMember(Value = "balanceSpot_request")]
        BalanceSpotRequest,

        [EnumMember(Value = "balanceSpot_subscribe")]
        BalanceSpotSubscribe,

        [EnumMember(Value = "balanceSpot_unsubscribe")]
        BalanceSpotUnsubscribe,

        [EnumMember(Value = "balanceMargin_request")]
        BalanceMarginRequest,

        [EnumMember(Value = "balanceMargin_subscribe")]
        BalanceMarginSubscribe,

        [EnumMember(Value = "balanceMargin_unsubscribe")]
        BalanceMarginUnsubscribe,

        [EnumMember(Value = "ordersPending_request")]
        ActiveOrdersRequest,

        [EnumMember(Value = "ordersPending_subscribe")]
        ActiveOrdersSubscribe,

        [EnumMember(Value = "ordersPending_unsubscribe")]
        ActiveOrdersUnsubscribe,

        [EnumMember(Value = "ordersExecuted_request")]
        ExecutedOrdersRequest,

        [EnumMember(Value = "ordersExecuted_subscribe")]
        ExecutedOrdersSubscribe,

        [EnumMember(Value = "ordersExecuted_unsubscribe")]
        ExecutedOrdersUnsubscribe,

        [EnumMember(Value = "deals_request")]
        UserTradesRequest,

        [EnumMember(Value = "deals_subscribe")]
        UserTradesSubscribe,

        [EnumMember(Value = "deals_unsubscribe")]
        UserTradesUnsubscribe,

        [EnumMember(Value = "candles_request")]
        CandlesRequest,

        [EnumMember(Value = "candles_subscribe")]
        CandlesSubscribe,

        [EnumMember(Value = "candles_unsubscribe")]
        CandlesUnsubscribe,

        [EnumMember(Value = "lastprice_request")]
        LastpriceRequest,

        [EnumMember(Value = "lastprice_subscribe")]
        LastpriceSubscribe,

        [EnumMember(Value = "lastprice_unsubscribe")]
        LastpriceUnsubscribe,

        [EnumMember(Value = "market_request")]
        Ticker24HRequest,

        [EnumMember(Value = "market_subscribe")]
        Ticker24HSubscribe,

        [EnumMember(Value = "market_unsubscribe")]
        Ticker24HUnsubscribe,

        [EnumMember(Value = "marketToday_request")]
        TickerUtcDayRequest,

        [EnumMember(Value = "marketToday_subscribe")]
        TickerUtcDaySubscribe,

        [EnumMember(Value = "marketToday_unsubscribe")]
        TickerUtcDayUnsubscribe,

        [EnumMember(Value = "trades_request")]
        PublicTradesRequest,

        [EnumMember(Value = "trades_subscribe")]
        PublicTradesSubscribe,

        [EnumMember(Value = "trades_unsubscribe")]
        PublicTradesUnsubscribe,

        [EnumMember(Value = "depth_request")]
        OrderBookRequest,

        [EnumMember(Value = "depth_subscribe")]
        OrderBookSubscribe,

        [EnumMember(Value = "depth_unsubscribe")]
        OrderBookUnsubscribe,
    }
    public enum SocketIncomeMethod
    {
        [EnumMember(Value = "balanceSpot_update")]
        BalanceSpot,

        [EnumMember(Value = "balanceMargin_update")]
        BalanceMargin,

        [EnumMember(Value = "ordersPending_update")]
        ActiveOrders,

        [EnumMember(Value = "ordersExecuted_update")]
        ExecutedOrders,

        [EnumMember(Value = "deals_update")]
        UserTrades,

        [EnumMember(Value = "candles_update")]
        Candles,

        [EnumMember(Value = "lastprice_update")]
        Lastprice,

        [EnumMember(Value = "market_update")]
        Ticker24H,

        [EnumMember(Value = "marketToday_update")]
        TickerUtcDay,

        [EnumMember(Value = "trades_update")]
        PublicTrades,

        [EnumMember(Value = "depth_update")]
        OrderBook,
    }
    public enum OrderBookSocketAggregationLevel
    {
        /// <summary>
        /// no interval
        /// </summary>
        NoAggregation = 0,
        /// <summary>
        /// "0.1"
        /// </summary>
        To1stDecimalPlace = -1,
        /// <summary>
        /// "0.01"
        /// </summary>
        To2ndDecimalPlace = -2,
        /// <summary>
        /// "0.001"
        /// </summary>
        To3rdDecimalPlace = -3,
        /// <summary>
        /// "0.0001"
        /// </summary>
        To4thDecimalPlace = -4,
        /// <summary>
        /// "0.00001"
        /// </summary>
        To5thDecimalPlace = -5,
        /// <summary>
        /// "0.000001"
        /// </summary>
        To6thDecimalPlace = -6,
        /// <summary>
        /// "0.0000001"
        /// </summary>
        To7thDecimalPlace = -7,
        /// <summary>
        /// "0.00000001"
        /// </summary>
        To8thDecimalPlace = -8
    }
}