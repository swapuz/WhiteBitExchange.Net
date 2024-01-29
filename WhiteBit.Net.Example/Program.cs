// See https://aka.ms/new-console-template for more information
using WhiteBit.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using WhiteBit.Net.Models.Enums;
using WhiteBit.Net.Models.Responses;
using WhiteBit.Net.Models.Requests;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using WhiteBit.Net.Clients;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Models;
using WhiteBit.Net.Interfaces;
using System.Net.Sockets;
using System.Security;
using CryptoExchange.Net.CommonObjects;

namespace WhiteBit.Net.Example
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var apikey = "803597747d5b814d1864f7c82a84134a";
            var secret = "933558d38d6c1ff73f21d62cd76fe6c9";
            var cl = new WhiteBitClientTests(apikey, secret);
            await cl.ExecuteWithdrawTest();
            //await Other();
        }

        private static async Task Other()
        {
            // var cred = new ApiCredentials("key", "secret"); // <- Provide you API key/secret in these fields to retrieve data related to your account


            var socketClient = new WhiteBitSocketClient(new WhiteBitSocketClientOptions()
            {
                // ApiCredentials = cred,
            }, null);
            // var result = await socketClient.SpotStreams.SubscribeToActiveOrders(data =>
            // {
            //     // foreach (var ordUpd in data)
            //     // {
            //     var order = data?.Order;
            //     Console.WriteLine($"Order {data?.Action} with {JsonSerializer.Serialize(order).ToString()}");
            //     // }
            // },
            // //     var result = await socket.SpotStreams.SubscribeToActiveOrders(data =>
            // //    {
            // //    var o = data?.Type == JTokenType.Object;
            // // foreach (var ordUpd in data)
            // //    var ds = data.ToObject<OrderSocketUpdate>();
            // // var order = data?.Order;
            // // Console.WriteLine($"Order {data?.Action} with {order?.Price} q-ty = {order?.Amount}");
            // // Console.WriteLine($"Order {data} with  q-ty = ");
            // // }
            // // },
            // default,
            // "DBTC_DUSDT", "BTC_USDT", "WAVES_USDT", "LINK_USDT");

            // var cl0 = (WhiteBitApiClientV4)cl.ApiClient;
            // var b0 = await cl.ApiClient.GetOrderTradesAsync(new GetOrderTradesRequest(1133469996));
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
            var socketBook = new WhiteBitSpotSymbolOrderBook("BTC_USD", new WhiteBitOrderBookOptions() { }, socketClient.SpotStreams);
            socketBook.OnBestOffersChanged += S_OnBestOffersChanged;
            await socketBook.StartAsync();
            Thread.Sleep(30000);
            await socketBook.StopAsync();
            Thread.Sleep(5000);

            await socketBook.StartAsync();

            Console.ReadLine();
        }

        static int c = 0;
        private static void S_OnBestOffersChanged((CryptoExchange.Net.Interfaces.ISymbolOrderBookEntry BestBid, CryptoExchange.Net.Interfaces.ISymbolOrderBookEntry BestAsk) obj)
        {
            Console.WriteLine($"S_OnBestOffersChanged:{obj.BestAsk.Price}:{obj.BestBid.Price}");
            c++;
            if (c==20)
            {
                c = 0;
                Console.WriteLine($"Changing bs:{obj.BestAsk.Price} to {obj.BestAsk.Price+50}");
                obj.BestAsk.Price =+ 50;
            }
        }
    }
    public class WhiteBitClientTests
    {
        private readonly WhiteBitRestClient _client;

        public WhiteBitClientTests(string ApiKey, string ApiSecret)
        {
            ApiCredentials cred;

            cred = new ApiCredentials(ApiKey, ApiSecret);
            var options = new WhiteBitRestClientOptions()
            {
                ApiCredentials = cred
            };
            _client = new WhiteBitRestClient(options, null);
        }

        public async Task ExecuteWithdrawTest()
        {
            //2742	487	1	100	10	"2023-03-14 08:25:16.028394"	"TJiVaRG1hNx4fVCN9TPZ29fKeoUnaC4ErJ"
            string syymbol = "BTC";
            decimal amount = 0.0011M;
            string address = "bc1q44q8jdagmqdyu20x8zx29873auqgsncaycrqnu";
            string uniqueId = "TJiZ29fKeoUnaC4ErJ0001";
            string memo = "0001";
            string network = "BTC";
            //params
            try
            {
                //                await _client.ApiClient.WithdrawRequest(order.To.Name, amount, order.AddressTo, withdraw.ID.ToString(), order.MemoTo, network);
                var WebsocketToken = await _client.ApiClient.WithdrawRequest(syymbol, amount, address, uniqueId, memo, network);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}