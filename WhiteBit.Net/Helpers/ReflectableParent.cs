using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace WhiteBit.Net.Helpers
{
    public abstract class ReflectableParent<T>
    where T : class
    {
        protected ReflectableParent(T parent)
        {
            InitInhertedProperties(parent);
        }

        /// <summary> copy base class instance's property values to this object. </summary>
        protected void InitInhertedProperties(T baseClassInstance)
        {
            if (baseClassInstance is null)
            {
                return;
            }
            foreach (PropertyInfo propertyInfo in baseClassInstance.GetType().GetProperties())
            {
                object? value = propertyInfo?.GetValue(baseClassInstance, null);
                if (null != value) 
                    propertyInfo!.SetValue(this, value);
            }
        }
    }
}