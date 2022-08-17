using CryptoExchange.Net;
using CryptoExchange.Net.Logging;
using CryptoExchange.Net.Objects;

namespace WhiteBit.Net
{
    public abstract class WhiteBitApiClient : RestApiClient
    {
        protected readonly WhiteBitClient baseClient;
        protected readonly Log log;

        protected WhiteBitApiClient(string name, BaseRestClientOptions options, RestApiClientOptions apiOptions, CryptoExchange.Net.Logging.Log log, WhiteBitClient client) : base(options, apiOptions)
        {
            ExchangeName = name;
            baseClient = client;
            this.log = log;

        }
        protected abstract string ApiVersion { get; }
        public string ExchangeName { get; }

        internal Uri GetUrl(string endpoint)
        {
            return new Uri(BaseAddress.AppendPath($"v{ApiVersion}").AppendPath(endpoint));
        }

    }
}