using UnityEngine;

namespace Juce.Utils.Singletons
{
    public abstract class AutoStartMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T singletonInstance;

        public static T Instance
        {
            get
            {
                if (singletonInstance == null)
                {
                    GameObject newGameObject = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(newGameObject);
                    singletonInstance = newGameObject.AddComponent<T>();
                }

                return singletonInstance;
            }
        }
    }
}