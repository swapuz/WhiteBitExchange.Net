using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using CryptoExchange.Net;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using Newtonsoft.Json;

namespace WhiteBit.Net
{
    internal class WhiteBitAuthenticationProvider : AuthenticationProvider
    {
        private static readonly object nonceLock = new object();
        private static long lastNonce;
        internal static string Nonce
        {
            get
            {
                lock (nonceLock)
                {
                    var nonce = (long)Math.Round((DateTime.UtcNow - DateTime.UnixEpoch).TotalMilliseconds);
                    if (nonce <= lastNonce)
                        nonce++;

                    lastNonce = nonce;
                    return lastNonce.ToString(CultureInfo.InvariantCulture);
                }
            }
        }

        public ApiCredentials Credentials { get;  set; }

        public WhiteBitAuthenticationProvider(ApiCredentials credentials) : base(credentials)
        {
        }

        public override void AuthenticateRequest(RestApiClient apiClient, Uri uri, HttpMethod method, Dictionary<string, object> providedParameters, bool auth, ArrayParametersSerialization arraySerialization, HttpMethodParameterPosition parameterPosition, out SortedDictionary<string, object> uriParameters, out SortedDictionary<string, object> bodyParameters, out Dictionary<string, string> headers)
        {
            uriParameters = parameterPosition == HttpMethodParameterPosition.InUri ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            bodyParameters = parameterPosition == HttpMethodParameterPosition.InBody ? new SortedDictionary<string, object>(providedParameters) : new SortedDictionary<string, object>();
            headers = new Dictionary<string, string>();

            if (!auth || method != HttpMethod.Post)
                return;

            bodyParameters.Add("request", uri.ToString().Split(uri.Host, StringSplitOptions.None)[1]);
            bodyParameters.Add("nonce", Nonce);

            headers.Add("X-TXC-APIKEY", Credentials.Key!.GetString());
            var payload = JsonConvert.SerializeObject(bodyParameters);
            var encodedPayload = Base64Encode(payload);
            headers.Add("X-TXC-PAYLOAD", encodedPayload);
            // lower case is necessary here but not documented
            headers.Add("X-TXC-SIGNATURE", SignHMACSHA512(encodedPayload).ToLower());
        
        
        }
        private string Base64Encode(string plainText)
        {
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            return Convert.ToBase64String(plainTextBytes);
        }
    }
}