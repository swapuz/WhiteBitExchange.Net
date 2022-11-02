using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
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
    public class SecondsToTimeSpanConverter : ObjectJsonConverter<TimeSpan>
    {
        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            return TimeSpan.FromSeconds(Convert.ToDouble(reader.Value));
        }
    }
}