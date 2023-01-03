using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using WhiteBit.Net.Clients.Options;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net.Clients
{
    public class WhiteBitClient : BaseRestClient, IWhiteBitClient
    {

        private const string BaseURL = " https://whitebit.com/api/";
        private const string ExchangeName = "WhiteBit";

        public WhiteBitClient(WhiteBitClientOptions options) : this(ExchangeName, options)
        {
        }
        public WhiteBitClient(string name, WhiteBitClientOptions options) : base(name, options)
        {
            ApiClient = AddApiClient(new WhiteBitApiClientV4(name, options, new RestApiClientOptions(BaseURL), log, this));
        }

        public IWhiteBitApiClientV4 ApiClient { get; }

        public ISpotClient CommonSpotClient => ApiClient;
   
    }
}