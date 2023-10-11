using UnityEngine;

public static class AutomaticFillComponents
{
    public static void DefineComponent<T>(MonoBehaviour parent, ref T target, ComponentLocationTypes componentType)
    {
        string type = target.GetType().ToString();

        type = GetShortType(type);

        if (componentType == ComponentLocationTypes.InChildren)
        {
            if (parent.GetComponentsInChildren<T>().Length < 1)
            {
                Debug.LogError($"{type} is not found");
            }
            else
            {
                if (parent.GetComponentsInChildren<T>().Length > 1)
                {
                    Debug.LogWarning($"{type} is too much! {type} length is {parent.GetComponentsInChildren<T>().Length}");
                }

                target = parent.GetComponentInChildren<T>();
            }
        }

        if (componentType == ComponentLocationTypes.InThis)
        {
            if (parent.GetComponents<T>().Length < 1)
            {
                Debug.LogError($"{type} is not found");
            }
            else
            {
                if (parent.GetComponents<T>().Length > 1)
                {
                    Debug.LogWarning($"{type} is too much! {type} length is {parent.GetComponents<T>().Length}");
                }

                target = parent.GetComponent<T>();
            }
        }
    }

    public static void DefineComponent<T>(MonoBehaviour parent, ref T[] targets)
    {
        string type = targets.GetType().ToString();

        type = GetShortType(type);

        if (parent.GetComponentsInChildren<T>().Length < 1)
        {
            Debug.LogError($"{type} is not found");
        }

        targets = parent.GetComponentsInChildren<T>();
    }

    private static string GetShortType(string longType)
    {
        string[] splitStrings = longType.Split(new char[] { '.' });
        int splitLenght = splitStrings.Length;
        return splitStrings[splitLenght - 1];
    }
}

public enum ComponentLocationTypes
{
    InChildren,
    InThis
}
