using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Models.Requests
{
    public class WhiteBitPlaceOrderRequest
    {
        private WhiteBitPlaceOrderRequest(WhiteBitOrderType type, string symbol, WhiteBitOrderSide side, decimal amount, string? clientOrderId = null, decimal? price = null, decimal? activationPrice = null)
        {
            Symbol = symbol;
            Side = side;
            Amount = amount;
            Price = price;
            ClientOrderId = clientOrderId;
            ActivationPrice = activationPrice;
            Type = type;
        }

        [JsonProperty("market")]
        public string Symbol { get; set; }

        [JsonProperty("side")]
        public WhiteBitOrderSide Side { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }
        
        [JsonProperty("price")]
        public decimal? Price { get; set; }

        [JsonProperty("clientOrderId")]
        public string? ClientOrderId { get; set; }
        [JsonProperty("activation_price")]
        public decimal? ActivationPrice { get; private set; }
        [JsonIgnore]
        public WhiteBitOrderType Type { get; private set; }

        /// <summary>
        /// creates limit trading order request
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="side">Order type. Variables: 'buy' / 'sell'</param>
        /// <param name="amount">Amount of stock (base) currency to buy or sell.</param>
        /// <param name="price">Price in money currency</param>
        /// <param name="clientOrderId">Identifier should be unique and contain letters, dashes or numbers. The identifier must be unique for the next 24 hours.</param>
        /// <returns></returns>
        public static WhiteBitPlaceOrderRequest CreateLimitOrderRequest(string symbol, WhiteBitOrderSide side, decimal amount, decimal price, string? clientOrderId = null) => new WhiteBitPlaceOrderRequest(WhiteBitOrderType.Limit, symbol, side, amount, clientOrderId, price);

        /// <summary>
        /// creates market trading order request
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="side">Order type. Variables: 'buy' / 'sell'</param>
        /// <param name="amount">⚠️ Amount of money (quote) currency to buy or amount in stock (base) currency to sell.
        /// Example: '5 USDT' for buy (min total) and '0.001 BTC' for sell (min amount).</param>
        /// <param name="clientOrderId">Identifier should be unique and contain letters, dashes or numbers. The identifier must be unique for the next 24 hours.</param>
        /// <returns></returns>
        public static WhiteBitPlaceOrderRequest CreateMarketOrderRequest(string symbol, WhiteBitOrderSide side, decimal amount, string? clientOrderId = null) => new WhiteBitPlaceOrderRequest(WhiteBitOrderType.Market, symbol, side, amount, clientOrderId);

        /// <summary>
        /// creates market trading order request
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="side">Order type. Variables: 'buy' / 'sell'</param>
        /// <param name="amount">⚠️ Amount in stock (base) currency for buy or sell. Example: 0.0001.</param>
        /// <param name="clientOrderId">Identifier should be unique and contain letters, dashes or numbers. The identifier must be unique for the next 24 hours.</param>
        /// <returns></returns>
        public static WhiteBitPlaceOrderRequest CreateStockMarketOrderRequest(string symbol, WhiteBitOrderSide side, decimal amount, string? clientOrderId = null) => new WhiteBitPlaceOrderRequest(WhiteBitOrderType.StockMarket, symbol, side, amount, clientOrderId);

        /// <summary>
        /// creates market trading order request
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="side">Order type. Variables: 'buy' / 'sell'</param>
        /// <param name="amount">⚠️ Amount of money (qoute) currency to buy or amount in stock (base) currency to sell. Example: 0.01 for buy and 0.0001 for sell.</param>
        /// <param name="activationPrice">Activation price in money currency. Example: 10000</param>
        /// <param name="clientOrderId">Identifier should be unique and contain letters, dashes or numbers. The identifier must be unique for the next 24 hours.</param>
        /// <returns></returns>
        public static WhiteBitPlaceOrderRequest CreateStopMarketOrderRequest(string symbol, WhiteBitOrderSide side, decimal amount, decimal activationPrice, string? clientOrderId = null) => new WhiteBitPlaceOrderRequest(WhiteBitOrderType.StopMarket, symbol, side, amount, clientOrderId, activationPrice: activationPrice);
        
        /// <summary>
        /// creates market trading order request
        /// </summary>
        /// <param name="symbol">Available market. Example: BTC_USDT</param>
        /// <param name="side">Order type. Variables: 'buy' / 'sell'</param>
        /// <param name="amount">Amount of stock (base) currency to buy or sell.</param>
        /// <param name="price">Price in money currency</param>
        /// <param name="activationPrice">Activation price in money currency. Example: 10000</param>
        /// <param name="clientOrderId">Identifier should be unique and contain letters, dashes or numbers. The identifier must be unique for the next 24 hours.</param>
        /// <returns></returns>
        public static WhiteBitPlaceOrderRequest CreateStopLimitOrderRequest(string symbol, WhiteBitOrderSide side, decimal amount, decimal price, decimal activationPrice, string? clientOrderId = null) => new WhiteBitPlaceOrderRequest(WhiteBitOrderType.StopLimit, symbol, side, amount, clientOrderId, price, activationPrice);
    }
}