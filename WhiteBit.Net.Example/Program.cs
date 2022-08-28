// See https://aka.ms/new-console-template for more information
using WhiteBit.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Responses;
using WhiteBit.Net.Models.Requests;

namespace WhiteBit.Net.Example
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var cl = new WhiteBitClient(new WhiteBitClientOptions()
            {
                ApiCredentials = new ApiCredentials("key", "secret"), // <- Provide you API key/secret in these fields to retrieve data related to your account
                LogLevel = LogLevel.Trace
            });
            // var cl0 = (WhiteBitApiClientV4)cl.ApiClient;
            var b0 = await cl.ApiClient.GetBalanceAsync("DBTC_DUS");
            // var a0 = await cl.ApiClient.GetExecutedOrdersAsync();
            // var t0 = await cl.ApiClient.GetTickersAsync();
            // var time = await cl0.GetServerTimestampTest();
            // var tr0 = await cl.ApiClient.GetPublicTradesAsync("BTCUSDT");
            // var tr1 = await cl.ApiClient.GetOwnTradesAsync();
            // var tr1 = await cl0.GetPublicTradesAsync("BTC_USDT", WhiteBitOrderSide.Buy);
            // var f0 = await cl.ApiClient.GetFuturesAsync();
            // var o1 = await cl.ApiClient.PlaceOrderAsync(WhiteBitPlaceOrderRequest.CreateLimitOrderRequest("DBTC_DUSDT", WhiteBitOrderSide.Buy, 0.0011m,5000));
            // var o0 = await cl.ApiClient.PlaceOrderAsync(WhiteBitPlaceOrderRequest.CreateStopLimitOrderRequest("DBTC_DUSDT", WhiteBitOrderSide.Sell, 0.0005m, 31011m, 25000m));
            // var o3 = await cl.ApiClient.PlaceOrderAsync(WhiteBitPlaceOrderRequest.CreateStopMarketOrderRequest("DBTC_DUSDT", WhiteBitOrderSide.Buy, 5.2m, 21630));
            // var o2 = await cl0.GetActiveOrdersAsync(new GetActiveOrdersRequest("DBTC_DUSDT", 112560248881));
            // var o1 = await cl0.GetActiveOrdersAsync(new GetActiveOrdersRequest("DBTC_DUSDT", 11256024888));
            // var o3 = await cl0.GetActiveOrdersAsync(new GetActiveOrdersRequest("DBTC_DUSDT"));
            // var o2 = await cl0.CancelOrderAsync("DBTC_DUSDT",112587180290);

            // var list = tr1.Data["DBTC_DUSDT"].ToList();
            // var r0 = b0.Data.ToList();
            // var r1 = t0.Data.ToList();
            // var r = tr0.Data?.ToList();
            Console.ReadLine();
        }
    }
}