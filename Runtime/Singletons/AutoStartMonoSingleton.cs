using UnityEngine;

namespace Juce.Utils.Singletons
{
    public abstract class AutoStartMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T singletonInstance;
        private static bool created;

        public static bool InstanceWasDestroyed => singletonInstance == null && created;

        public static T Instance
        {
            get
            {
                if (singletonInstance == null)
                {
                    if (created)
                    {
                        return null;
                    }

                    GameObject newGameObject = new GameObject(typeof(T).Name);
                    DontDestroyOnLoad(newGameObject);
                    singletonInstance = newGameObject.AddComponent<T>();

                    created = true;
                }

                return singletonInstance;
            }
        }
    }
}