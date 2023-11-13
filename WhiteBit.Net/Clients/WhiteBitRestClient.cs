using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net.Clients
{
    public class WhiteBitRestClient : BaseRestClient, IWhiteBitClient
    {

        private const string BaseURL = " https://whitebit.com/api/";
        private const string ExchangeName = "WhiteBit";

        //public WhiteBitRestClient(WhiteBitRestClientOptions options) : this(ExchangeName, options)
        //{
        //}
        public WhiteBitRestClient(WhiteBitRestClientOptions options, ILoggerFactory? loggerFactory, string name = ExchangeName) : base(loggerFactory ,name)
        {
            ApiClient = AddApiClient(new WhiteBitApiClientV4(name, options, new RestApiClientOptions(BaseURL), _logger, this));
        }

        public IWhiteBitApiClientV4 ApiClient { get; }

        public ISpotClient CommonSpotClient => ApiClient;
   
    }
}