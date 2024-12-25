using UnityEngine;

namespace Tools.Utils.FillComponents
{
    public static class AutomaticFillComponents
    {
        public static void DefineComponent<T>(MonoBehaviour parent, ref T target, ComponentLocationTypes componentType) where T: class
        {
            if (target is not null)
            {
                if (target.ToString().Equals("null") == false)
                    return;
            }

            string type = GetShortType<T>();

            if (componentType == ComponentLocationTypes.InThisElseChildren)
            {
                if (parent.GetComponents<T>().Length < 1)
                {
                    componentType = ComponentLocationTypes.InChildren;
                }
                else
                {
                    componentType = ComponentLocationTypes.InThis;
                }
            }

            if (componentType == ComponentLocationTypes.InChildren)
            {
                if (parent.GetComponentsInChildren<T>(true).Length - parent.GetComponents<T>().Length < 1)
                {
                    Debug.LogError($"{type} is not found. Parent: " + parent.ToString());
                }
                else
                {
                    if (parent.GetComponentsInChildren<T>(true).Length - parent.GetComponents<T>().Length > 1)
                    {
                        Debug.LogWarning($"{type} is too much! {type} length is {parent.GetComponentsInChildren<T>(true).Length - parent.GetComponents<T>().Length}. Parent: {parent.ToString()}");
                    }

                    if (parent.GetComponentsInChildren<T>(true).Length != 1)
                    {
                        T[] targets = parent.GetComponentsInChildren<T>(true);
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
                                ShowSuccessMessage(type, parent);
                                break;
                            }
                        }
                    }
                    else
                    {
                        target = parent.GetComponentInChildren<T>(true);
                        ShowSuccessMessage(type, parent);
                    }
                }
            }

            if (componentType == ComponentLocationTypes.InThis)
            {
                if (parent.GetComponents<T>().Length < 1)
                {
                    Debug.LogError($"{type} is not found. Parent: " + parent.ToString());
                }
                else
                {
                    if (parent.GetComponents<T>().Length > 1)
                    {
                        Debug.LogWarning($"{type} is too much! {type} length is {parent.GetComponents<T>().Length}.  Parent: " + parent.ToString());
                    }

                    target = parent.GetComponent<T>();
                    ShowSuccessMessage(type, parent);
                }
            }

            if (componentType == ComponentLocationTypes.InScene)
            {
                Object[] targets = Object.FindObjectsOfType(typeof(T));

                if (targets.Length < 1)
                {
                    Debug.LogError($"{type} is not found.  Parent: " + parent.ToString());
                }
                else
                {
                    if (targets.Length > 1)
                    {
                        Debug.LogWarning($"{type} is too much! {type} length is {targets.Length}. Parent: " + parent.ToString());
                    }

                    target = targets[0] as T;
                    ShowSuccessMessage(type, parent);
                }
            }
        }

        public static void DefineComponent<T>(MonoBehaviour parent, ref T[] targets)
        {
            string type = GetShortType<T>();

            if (parent.GetComponentsInChildren<T>(true).Length < 1)
            {
                Debug.LogError($"{type} is not found. Parent: " + parent.ToString());
            }

            targets = parent.GetComponentsInChildren<T>(true);
            ShowSuccessMessage(type, parent);
        }

        private static string GetShortType<T>()
        {
            string longType = typeof(T).ToString();
            string[] splitStrings = longType.Split(new char[] { '.' });
            int splitLenght = splitStrings.Length;
            return splitStrings[splitLenght - 1];
        }

        private static void ShowSuccessMessage(string type, MonoBehaviour parent)
        {
            Debug.Log($"{type} successfully found ! Parent: " + parent.ToString());
        }
    }
}