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

namespace WhiteBit.Net.Example
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
             var cred = new ApiCredentials("key", "secret"); // <- Provide you API key/secret in these fields to retrieve data related to your account

            var cl = new WhiteBitRestClient(new WhiteBitRestClientOptions()
             {
                 ApiCredentials =  cred,
                 
             });
            var res = await cl.ApiClient.GetFeeAsync();
            if (res.Success)
            {
                
            }
            
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
}