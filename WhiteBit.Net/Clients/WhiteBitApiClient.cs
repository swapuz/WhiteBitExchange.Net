using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WhiteBit.Net.Clients.Options;

namespace WhiteBit.Net.Clients
{
    public abstract class WhiteBitApiClient : RestApiClient
    {
        protected readonly WhiteBitRestClient baseClient;
        public JsonSerializerSettings DefaultSerializerSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };
        public WhiteBitRestClientOptions Options { get; set; }
        private const string _baseURL = " https://whitebit.com/api/";
        private const string _ExchangeName = "WhiteBit";

        protected WhiteBitApiClient(string name, WhiteBitRestClientOptions options, RestApiClientOptions apiOptions, ILogger log, WhiteBitRestClient client) 
            : base(log, null, _baseURL, options, apiOptions)
        {
            baseClient = client;
            Options = options;
        }
        protected abstract string ApiVersion { get; }
        public string ExchangeName { get; } = _ExchangeName;

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
        protected async Task<WebCallResult<T>> SendRequestInternal<T>(
               Uri uri,
               HttpMethod method,
               CancellationToken cancellationToken,
               Dictionary<string, object>? parameters = null,
               bool signed = false,
               RequestBodyFormat? requestBodyFormat = null,
               HttpMethodParameterPosition? postPosition = null,
               ArrayParametersSerialization? arraySerialization = null,
               int weight = 1,
               JsonSerializer? deserializer = null
           ) where T : class
        {
            return await base.SendRequestAsync<T>(uri, method, cancellationToken, parameters, signed, requestBodyFormat, postPosition, arraySerialization, requestWeight: weight, deserializer);
        }
    }
}