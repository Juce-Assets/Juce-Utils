using Juce.Utils.Contracts;
using UnityEngine;

namespace Juce.Utils.Singletons
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T singletonInstance;

        protected void InitInstance(T instance)
        {
            Contract.IsNotNull(instance, $"Instance of type {nameof(T)} cannot be null");

            singletonInstance = instance;
        }

        public static T Instance
        {
            get
            {
                Contract.IsNotNull(singletonInstance, $"{nameof(MonoSingleton<T>)} used before initialization. " +
                       $"Please use {nameof(InitInstance)} before using the singleton instance");

                return singletonInstance;
            }
        }
    }
}