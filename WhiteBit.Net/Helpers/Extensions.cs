using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using CryptoExchange.Net.CommonObjects;
using Newtonsoft.Json;
using WhiteBit.Net.Interfaces;
using WhiteBit.Net.Models.Enums;

namespace WhiteBit.Net.Helpers
{
    public static class Extensions
    {
        const BindingFlags boundingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        private static string? Normalize(this decimal? value)
        {
            if (!value.HasValue)
            {
                return null;
            }
            return (value.Value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }
        public static Dictionary<string, object> AsDictionary(this object source,
          BindingFlags bindingAttr = BindingFlags.FlattenHierarchy |
          BindingFlags.Instance |
        //   BindingFlags.NonPublic |
          BindingFlags.Public |
          BindingFlags.Static)
        {
            try
            {
                var result = new Dictionary<string, object>();
                var props = source.GetType().GetProperties(bindingAttr);
                foreach (var p in props)
                {
                    if (p.IsDefined(typeof(JsonIgnoreAttribute)))
                        continue;
                    string key = p.Name;
                    if (p.IsDefined(typeof(JsonPropertyAttribute)))
                    {
                        key = p.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? p.Name;
                    }
                    object? value = p.GetValue(source, null);

                    if (value is null)
                    {
                        continue;
                    }

                    if (value is bool)
                    {
                        value = value.ToString()!.ToLowerInvariant();
                    }
                    if (value is decimal || value is decimal?)
                    {
                        value = (value as decimal?).Normalize();

                    }
                    if (value?.GetType().IsEnum == true)
                    {
                        var prop = value!.GetType().GetField(value!.ToString()!);
                        if (prop?.IsDefined(typeof(EnumMemberAttribute)) == true)
                        {
                            value = prop.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? value;
                        }
                    }
                    if (p.IsDefined(typeof(JsonConverterAttribute)))
                    {
                        var att = p.GetCustomAttribute<JsonConverterAttribute>();
                        var t = Activator.CreateInstance(att!.ConverterType);
                        value = JsonConvert.SerializeObject(value, (t as JsonConverter)!);
                    }
                    if (!result.ContainsKey(key) && !String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value?.ToString()))
                    {
                        result.Add(key, value);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Create instance of T based on source properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target">you can initialize object with predefined values of nullable (or not exist in source obj) properties.</param>
        /// <returns></returns>
        internal static T? Convert<T>(this IConvertible<T> source, T? target = null)
        where T : class,new()
        {
            if (source is null)
                return null;
            var result = target ?? new T();
            var sourcePropNames = source.GetType().GetProperties(boundingFlags).Where(prop => prop.CanRead).ToDictionary(p => p.Name, p => p);
            foreach (PropertyInfo propertyInfo in result.GetType().GetProperties(boundingFlags).Where(prop => prop.CanWrite))
            {
                if (sourcePropNames.TryGetValue(propertyInfo.Name, out var sProp))
                {
                    var value = sProp.GetValue(source);
                    if (value != null)
                    {
                        var sType = Nullable.GetUnderlyingType(sProp.PropertyType) ?? sProp.PropertyType;
                        var tType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

                        if (sType == tType)
                            propertyInfo!.SetValue(result, value);
                        else
                            propertyInfo!.SetValue(result, System.Convert.ChangeType(value, tType));
                    }
                }
            }
            return result;
        }
        public static WhiteBitOrderSide ToWhiteBitOrderSide(this CommonOrderSide source)
        {
            return source switch
            {
                CommonOrderSide.Sell => WhiteBitOrderSide.Sell,
                _ => WhiteBitOrderSide.Buy
            };
        }

        internal static bool DoesMethodMatch(this SocketOutgoingMethod methodIn, SocketIncomeMethod? methodOut)
        {
            switch (methodIn)
            {
                case SocketOutgoingMethod.ActiveOrdersSubscribe when methodOut == SocketIncomeMethod.ActiveOrders:
                case SocketOutgoingMethod.TickerSubscribe when methodOut == SocketIncomeMethod.Ticker:
                case SocketOutgoingMethod.CandlesSubscribe when methodOut == SocketIncomeMethod.Candles:
                case SocketOutgoingMethod.LastpriceSubscribe when methodOut == SocketIncomeMethod.Lastprice:
                case SocketOutgoingMethod.OrderBookSubscribe when methodOut == SocketIncomeMethod.OrderBook:
                case SocketOutgoingMethod.UserTradesSubscribe when methodOut == SocketIncomeMethod.UserTrades:
                case SocketOutgoingMethod.BalanceSpotSubscribe when methodOut == SocketIncomeMethod.BalanceSpot:
                case SocketOutgoingMethod.PublicTradesSubscribe when methodOut == SocketIncomeMethod.PublicTrades:
                case SocketOutgoingMethod.BalanceMarginSubscribe when methodOut == SocketIncomeMethod.BalanceMargin:
                case SocketOutgoingMethod.ExecutedOrdersSubscribe when methodOut == SocketIncomeMethod.ExecutedOrders:
                case SocketOutgoingMethod.MarketStatisticSubscribe when methodOut == SocketIncomeMethod.MarketStatistic:
                    return true;
                default:
                    return false;
            }
        }
        internal static SocketOutgoingMethod? GetCorrespondingUnsubscribeMethod(this SocketOutgoingMethod source)
        {
            return source switch
            {
                SocketOutgoingMethod.ActiveOrdersSubscribe => SocketOutgoingMethod.ActiveOrdersUnsubscribe,
                SocketOutgoingMethod.TickerSubscribe => SocketOutgoingMethod.TickerUnsubscribe,
                SocketOutgoingMethod.CandlesSubscribe => SocketOutgoingMethod.CandlesUnsubscribe,
                SocketOutgoingMethod.LastpriceSubscribe => SocketOutgoingMethod.LastpriceUnsubscribe,
                SocketOutgoingMethod.OrderBookSubscribe => SocketOutgoingMethod.OrderBookUnsubscribe,
                SocketOutgoingMethod.UserTradesSubscribe => SocketOutgoingMethod.UserTradesUnsubscribe,
                SocketOutgoingMethod.BalanceSpotSubscribe => SocketOutgoingMethod.BalanceSpotUnsubscribe,
                SocketOutgoingMethod.PublicTradesSubscribe => SocketOutgoingMethod.PublicTradesUnsubscribe,
                SocketOutgoingMethod.BalanceMarginSubscribe => SocketOutgoingMethod.BalanceMarginUnsubscribe,
                SocketOutgoingMethod.ExecutedOrdersSubscribe => SocketOutgoingMethod.ExecutedOrdersUnsubscribe,
                SocketOutgoingMethod.MarketStatisticSubscribe => SocketOutgoingMethod.MarketStatisticUnsubscribe,
                _ => null
            };
        }
    }
}