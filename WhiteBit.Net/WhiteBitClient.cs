using CryptoExchange.Net;
using CryptoExchange.Net.Interfaces.CommonClients;
using CryptoExchange.Net.Objects;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net
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
        internal async Task<WebCallResult<T>> SendRequestInternal<T>(
            RestApiClient apiClient,
            Uri uri,
            HttpMethod method,
            CancellationToken cancellationToken,
            Dictionary<string, object>? parameters = null,
            bool signed = false,
            HttpMethodParameterPosition? postPosition = null,
            ArrayParametersSerialization? arraySerialization = null,
            int weight = 1
        ) where T : class
        {
            return await base.SendRequestAsync<T>(apiClient, uri, method, cancellationToken, parameters, signed, postPosition, arraySerialization, requestWeight: weight);
        }
    }
}