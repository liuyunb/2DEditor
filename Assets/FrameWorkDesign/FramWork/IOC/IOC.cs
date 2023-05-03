using System;
using System.Collections.Generic;

namespace FrameWorkDesign
{
    public class IOC
    {
        private Dictionary<Type, object> _instance = new Dictionary<Type, object>();

        public void Register<T>(T instance) where T : class
        {
            var key = typeof(T);
            if (_instance.ContainsKey(key))
            {
                _instance[key] = instance;
            }
            else
            {
                _instance.Add(key,instance);
            }
        }

        public T Get<T>() where T : class
        {
            var key = typeof(T);
            object res;
            if (_instance.TryGetValue(key, out res))
            {
                return res as T; 
            }

            return null;

        }
        
    }
}
