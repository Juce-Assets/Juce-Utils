using UnityEngine;

public static class JuceUtilsGameObjectExtensions
{
    public static T GetOrAddComponent<T>(this GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();

        if (component != null)
        {
            return component;
        }

        return go.AddComponent<T>();
    }

    public static GameObject Instantiate(this GameObject go, Transform parent = null)
    {
        return MonoBehaviour.Instantiate(go, parent);
    }

    public static T InstantiateAndGetComponent<T>(this GameObject go, Transform parent = null) where T : Component
    {
        GameObject goInstance = MonoBehaviour.Instantiate(go, parent);

        return goInstance.GetComponent<T>();
    }

    public static void Destroy(this GameObject go)
    {
        MonoBehaviour.Destroy(go);
    }

    public static void SetParent(this GameObject go, GameObject parent)
    {
        if (parent == null)
        {
            go.transform.SetParent(null);
        }
        else
        {
            go.transform.SetParent(parent.transform);
        }
    }

    public static void RemoveParent(this GameObject go)
    {
        go.transform.SetParent(null);
    }
}