using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Tools.Utils.FillComponents
{
    public static class AutomaticFillComponents
    {
        // Возвращаемое значение:
        //  0 - компонент уже задан (не пуст), нет смысла брыкаться
        //  1 - успешно найдено
        // -1 - ошибка
        //  2 - успешно найдено, по компоненотов много, не факт что применился тот что задумывался, прикрепился первый попавшийся
        // -2 - компонент не найден ВО ВСЕЙ СЦЕНЕ
        //  3 - массив компонентов успешно задан (проигнорены уже заданные значения, задались заного (значения 0 для массива нет))
        // -3 - компилятор не хочет компилиться без полного return на метод, до сюда дойти никогда не должно
        public static ComponentAttachInfo DefineComponent<T>(MonoBehaviour parent, ref T target, ComponentLocationTypes componentType) where T: class
        {
            Debug.Log(parent.ToString());
            Debug.Log(target.ToString());

            if (target is not null)
            {
                if (target.ToString().Equals("null") == false)
                    return new ComponentAttachInfo(parent.ToString(), target.ToString(), 0);
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
                    return new ComponentAttachInfo(parent.ToString(), target.ToString(), -1);
                }
                else
                {
                    int warningReturnSuccess = 1;

                    if (parent.GetComponentsInChildren<T>(true).Length - parent.GetComponents<T>().Length > 1)
                    {
                        Debug.LogWarning($"{type} is too much! {type} length is {parent.GetComponentsInChildren<T>(true).Length - parent.GetComponents<T>().Length}. Parent: {parent.ToString()}");
                        warningReturnSuccess = 2;
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
                                return new ComponentAttachInfo(parent.ToString(), target.ToString(), warningReturnSuccess);
                            }
                        }
                    }
                    else
                    {
                        target = parent.GetComponentInChildren<T>(true);
                        ShowSuccessMessage(type, parent);
                        return new ComponentAttachInfo(parent.ToString(), target.ToString(), 1);
                    }
                }
            }

            if (componentType == ComponentLocationTypes.InThis)
            {
                if (parent.GetComponents<T>().Length < 1)
                {
                    Debug.LogError($"{type} is not found. Parent: " + parent.ToString());
                    return new ComponentAttachInfo(parent.ToString(), target.ToString(), -1);
                }
                else
                {
                    int warningReturnSuccess = 1;

                    if (parent.GetComponents<T>().Length > 1)
                    {
                        Debug.LogWarning($"{type} is too much! {type} length is {parent.GetComponents<T>().Length}.  Parent: " + parent.ToString());
                        warningReturnSuccess = 2;
                    }

                    target = parent.GetComponent<T>();
                    ShowSuccessMessage(type, parent);
                    return new ComponentAttachInfo(parent.ToString(), target.ToString(), warningReturnSuccess);
                }
            }

            if (componentType == ComponentLocationTypes.InScene)
            {
                Object[] targets = Object.FindObjectsOfType(typeof(T));

                if (targets.Length < 1)
                {
                    Debug.LogError($"{type} is not found.  Parent: " + parent.ToString());
                    return new ComponentAttachInfo(parent.ToString(), target.ToString(), -2);
                }
                else
                {
                    int warningReturnSuccess = 1;

                    if (targets.Length > 1)
                    {
                        Debug.LogWarning($"{type} is too much! {type} length is {targets.Length}. Parent: " + parent.ToString());
                        warningReturnSuccess = 2;
                    }

                    target = targets[0] as T;
                    ShowSuccessMessage(type, parent);
                    return new ComponentAttachInfo(parent.ToString(), target.ToString(), warningReturnSuccess);
                }
            }

            return new ComponentAttachInfo(parent.ToString(), target.ToString(), -3);
        }

        public static ComponentAttachInfo DefineComponent<T>(MonoBehaviour parent, ref T[] targets)
        {
            if (targets is null)
            {
                Debug.LogError($"targets is null. Parent: " + parent.ToString());
                return new ComponentAttachInfo(parent.ToString(), targets.ToString(), -1);
            }

            if (targets.Length < 1)
            {
                Debug.LogError($"targets.Length < 1. Parent: " + parent.ToString());
                return new ComponentAttachInfo(parent.ToString(), targets.ToString(), -1);
            }

            string type = GetShortType<T>();

            if (parent.GetComponentsInChildren<T>().Length < 1)
            {
                Debug.LogError($"{type} is not found. Parent: " + parent.ToString());
                return new ComponentAttachInfo(parent.ToString(), targets.ToString(), -1);
            }

            targets = parent.GetComponentsInChildren<T>();
            ShowSuccessMessage(type, parent);
            return new ComponentAttachInfo(parent.ToString(), targets.ToString(), 3);
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