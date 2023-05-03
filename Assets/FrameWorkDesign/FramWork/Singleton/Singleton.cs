using System;
using System.Reflection;

namespace FrameWorkDesig
{
    public class Singleton<T> where T : Singleton<T>
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    var type = typeof(T);
                    var ctors = type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic);
                    var c = Array.Find(ctors, ctor => ctor.GetParameters().Length == 0);

                    if (c == null)
                    {
                        throw new Exception("Non Public Constructors Not Found in " + type.Name);
                    }

                    _instance = c.Invoke(null) as T;
                }

                return _instance;
            }
        }
    }
}
