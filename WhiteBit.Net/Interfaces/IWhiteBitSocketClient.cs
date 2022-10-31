using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoExchange.Net.Interfaces;

namespace WhiteBit.Net.Interfaces
{
    public interface IWhiteBitSocketClient: ISocketClient
    {
        /// <summary>
        /// Spot streams
        /// </summary>
        IWhiteBitSocketClientSpotStream SpotStreams { get; }
        // IWhiteBitSocketClientMarginStreams MarginStreams { get; }
    }
}