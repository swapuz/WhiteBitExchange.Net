using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WhiteBit.Net.Interfaces;

namespace WhiteBit.Net.Helpers
{
    public static class Extensions
    {
        private static string? Normalize(this decimal? value)
        {
            if (!value.HasValue)
            {
                return null;
            }
            return (value.Value / 1.000000000000000000000000000000000m).ToString(CultureInfo.InvariantCulture);
        }
        public static Dictionary<string, object> AsDictionary(this object source,
          BindingFlags bindingAttr = BindingFlags.FlattenHierarchy |
          BindingFlags.Instance |
        //   BindingFlags.NonPublic |
          BindingFlags.Public |
          BindingFlags.Static)
        {
            try
            {
                var result = new Dictionary<string, object>();
                var props = source.GetType().GetProperties(bindingAttr);
                foreach (var p in props)
                {
                    if (p.IsDefined(typeof(JsonIgnoreAttribute)))
                        continue;
                    string key = p.Name;
                    if (p.IsDefined(typeof(JsonPropertyAttribute)))
                    {
                        key = p.GetCustomAttribute<JsonPropertyAttribute>()?.PropertyName ?? p.Name;
                    }
                    object? value = p.GetValue(source, null);

                    if (value is null)
                    {
                        continue;
                    }

                    if (value is bool)
                    {
                        value = value.ToString()!.ToLowerInvariant();
                    }
                    if (value is decimal || value is decimal?)
                    {
                        value = (value as decimal?).Normalize();

                    }
                    if (value?.GetType().IsEnum == true)
                    {
                        var prop = value!.GetType().GetField(value!.ToString()!);
                        if (prop?.IsDefined(typeof(EnumMemberAttribute)) == true)
                        {
                            value = prop.GetCustomAttribute<EnumMemberAttribute>()?.Value ?? value;
                        }
                    }
                    if (p.IsDefined(typeof(JsonConverterAttribute)))
                    {
                        var att = p.GetCustomAttribute<JsonConverterAttribute>();
                        var t = Activator.CreateInstance(att!.ConverterType);
                        value = JsonConvert.SerializeObject(value, (t as JsonConverter)!);
                    }
                    if (!result.ContainsKey(key) && !String.IsNullOrEmpty(key) && !String.IsNullOrEmpty(value?.ToString()))
                    {
                        result.Add(key, value);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Create instance of T based on source properties.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="target">you can initialize object with predefined values of nullable (or not exist in source obj) properties.</param>
        /// <returns></returns>
        public static T? Convert<T>(this IConvertible<T> source, T? target = null)
        where T : class,new()
        {
            if (source is null)
                return null;
            var result = target ?? new T();
            var targPropNames = result.GetType().GetProperties().Where(prop => prop.CanWrite).Select(prop => prop.Name).ToList();
            foreach (PropertyInfo propertyInfo in source.GetType().GetProperties().Where(prop => prop.CanRead))
            {
                if (!targPropNames.Contains(propertyInfo.Name))
                    continue;
                var value = propertyInfo?.GetValue(source);
                if (null != value )
                    propertyInfo!.SetValue(result, value);
            }
            return result;
        }
    }
}