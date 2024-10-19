using UnityEngine;

namespace Tools.Utils.FillComponents
{
    public static class AutomaticFillComponents
    {
        public static void DefineComponent<T>(MonoBehaviour parent, ref T target, ComponentLocationTypes componentType) where T: class
        {
            string type = GetShortType<T>();

            if (componentType == ComponentLocationTypes.InChildren)
            {
                if (parent.GetComponentsInChildren<T>().Length - parent.GetComponents<T>().Length < 1)
                {
                    Debug.LogError($"{type} is not found");
                }
                else
                {
                    if (parent.GetComponentsInChildren<T>().Length - parent.GetComponents<T>().Length > 1)
                    {
                        Debug.LogWarning($"{type} is too much! {type} length is {parent.GetComponentsInChildren<T>().Length - parent.GetComponents<T>().Length}");
                    }

                    if (parent.GetComponentsInChildren<T>().Length != 1)
                    {
                        T[] targets = parent.GetComponentsInChildren<T>();
                        T[] inThisTargets = parent.GetComponents<T>();
                        bool isInThis;

                        foreach (T variant in targets)
                        {
                            isInThis = false;

                            foreach (T inThisVariant in inThisTargets)
                            {
                                if (variant == inThisVariant)
                                {
                                    isInThis = true;
                                }
                            }

                            if (isInThis == false)
                            {
                                target = variant;
                                ShowSuccessMessage(type);
                                break;
                            }
                        }
                    }
                    else
                    {
                        target = parent.GetComponentInChildren<T>();
                        ShowSuccessMessage(type);
                    }
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

            if (componentType == ComponentLocationTypes.InScene)
            {
                Object[] targets = Object.FindObjectsOfType(typeof(T));

                if (targets.Length < 1)
                {
                    Debug.LogError($"{type} is not found");
                }
                else
                {
                    if (targets.Length > 1)
                    {
                        Debug.LogWarning($"{type} is too much! {type} length is {targets.Length}");
                    }

                    target = targets[0] as T;
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
}