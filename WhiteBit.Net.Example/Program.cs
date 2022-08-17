// See https://aka.ms/new-console-template for more information
using WhiteBit.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;

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
            var b0 = await cl.ApiClient.GetBalancesAsync();
            var b1 = await cl.ApiClient.GetBalancesAsync("BTC");
            var t0 = await cl.ApiClient.GetTickersAsync();
            Console.ReadLine();
        }
    }
}