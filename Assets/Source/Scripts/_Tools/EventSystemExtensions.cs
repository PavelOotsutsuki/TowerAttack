using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace Tools
{
    public static class EventSystemExtensions
    {
        public static bool TryGetComponentInRaycasts<T>(this EventSystem system, PointerEventData eventData, out T findedComponent)
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();

            system.RaycastAll(eventData, raycastResults);
            findedComponent = default;

            foreach (RaycastResult raycastResult in raycastResults)
            {
                if (raycastResult.gameObject.TryGetComponent(out T component))
                {
                    findedComponent = component;
                    return true;
                }
            }

            return false;
        }
    }
}
