using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools
{
    public static class EventSystemExtensions
    {
        public static bool TryGetComponentInRaycasts<T>(this EventSystem system, PointerEventData eventData, out T findedComponent) where T : class
        {
            List<RaycastResult> raycastResults = new List<RaycastResult>();

            system.RaycastAll(eventData, raycastResults);
            findedComponent = null;

            Debug.Log(raycastResults.Count);
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
