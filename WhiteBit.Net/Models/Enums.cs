using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhiteBit.Net.Models.Enums
{
    /// <summary>
    /// The side of an order
    /// </summary>
    public enum WhiteBitOrderSide
    {
        /// <summary>
        /// Buy
        /// </summary>
        Buy,
        /// <summary>
        /// Sell
        /// </summary>
        Sell
    }
    public enum WhiteBitProductType
    {
        Futures,
        Perpetual,
        Options

    }
}