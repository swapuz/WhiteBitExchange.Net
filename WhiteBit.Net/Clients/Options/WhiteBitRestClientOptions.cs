using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBit.Net.Clients.Options
{
    public class WhiteBitRestClientOptions : RestExchangeOptions<WhiteBitEnvironment>
    {
    }
    public class RestApiClientOptions : RestApiOptions 
    {
        private string _baseAddress;

        public RestApiClientOptions()
        {

        }
        public RestApiClientOptions(string baseAddress)
        {
            _baseAddress = baseAddress;
        }
    }

}
