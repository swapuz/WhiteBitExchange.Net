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
        /// <summary>
        /// Fill parameters in a path. Parameters are specified by '{}' and should be specified in occuring sequence
        /// </summary>
        /// <param name="path">The total path string</param>
        /// <param name="values">The values to fill</param>
        /// <returns></returns>
        protected static string FillPathParameter(string path, params string[] values)
        {
            foreach (var value in values)
            {
                var indexB = path.IndexOf("{", StringComparison.Ordinal);
                var indexE = path.IndexOf("}", StringComparison.Ordinal);
                if (indexB >= 0 && indexE > indexB)
                {
                    path = path.Remove(indexB, indexE - indexB +1);
                    path = path.Insert(indexB, value);
                }
            }
            return path;
        }

    }
}