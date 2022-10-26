using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WhiteBit.Net.Interfaces
{
    public interface IWhiteBitSocketDataMethod<TMethod>
    {
        int? Id { get; set; }
        TMethod Method { get; set; }
    }
}