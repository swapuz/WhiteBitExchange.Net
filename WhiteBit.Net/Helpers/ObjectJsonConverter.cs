using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Converters;

namespace WhiteBit.Net.Helpers
{
    public class ObjectJsonConverter<TType> : CustomCreationConverter<TType>
    where TType : new()
    {
        public override TType Create(Type objectType)
        {
            return new();
        }
    }
}