using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net
{
    public class WhiteBitClient : BaseRestClient, IWhiteBitClient
    {
        public WhiteBitClient(WhiteBitClientOptions options) : base("WhiteBit", options)
        {
        }
        public WhiteBitClient(string name, WhiteBitClientOptions options) : base(name, options)
        {
        }
    }
}