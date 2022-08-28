using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhiteBit.Net.Interfaces
{
    /// <summary>
    /// no members needed, just use the interface in extension methods
    /// </summary>
    /// <typeparam name="Ttarget"></typeparam>
    public interface IConvertible<Ttarget>
    where Ttarget : class
    {
    }
}