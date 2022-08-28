using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.CommonClients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiteBit.Net.Interfaces
{
    public interface IWhiteBitClient: IRestClient
    {
        IWhiteBitApiClientV4 ApiClient { get; }
        ISpotClient CommonSpotClient { get; }
    }
}
