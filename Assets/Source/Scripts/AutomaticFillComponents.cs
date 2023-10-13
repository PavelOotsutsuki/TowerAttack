using UnityEngine;

public static class AutomaticFillComponents
{
    public static void DefineComponent<T>(MonoBehaviour parent, ref T target, ComponentLocationTypes componentType)
    {
        string type = GetShortType<T>();

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
                ShowSuccessMessage(type);
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
                ShowSuccessMessage(type);
            }
        }
    }

    public static void DefineComponent<T>(MonoBehaviour parent, ref T[] targets)
    {
        string type = GetShortType<T>();

        if (parent.GetComponentsInChildren<T>().Length < 1)
        {
            Debug.LogError($"{type} is not found");
        }

        targets = parent.GetComponentsInChildren<T>();
        ShowSuccessMessage(type);
    }

    private static string GetShortType<T>()
    {
        string longType = typeof(T).ToString();
        string[] splitStrings = longType.Split(new char[] { '.' });
        int splitLenght = splitStrings.Length;
        return splitStrings[splitLenght - 1];
    }

    private static void ShowSuccessMessage(string type)
    {
        Debug.Log($"{type} successfully found !");
    }
}

public enum ComponentLocationTypes
{
    InChildren,
    InThis
}
