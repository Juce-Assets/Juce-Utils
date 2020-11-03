using System;

namespace Juce.Utils.Singletons
{
    public class Singleton<T> where T : class
    {
        private static T singletonInstance;

        public static T Instance
        {
            get
            {
                if (singletonInstance == null)
                {
                    singletonInstance = Activator.CreateInstance<T>();
                }

                return singletonInstance;
            }
        }
    }
}