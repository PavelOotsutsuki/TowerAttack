using System;
using TMPro;
using UnityEngine.EventSystems;

namespace Tools.UI.Extendeds
{
    public class ExtendedTMP_InputField : TMP_InputField
    {
        //private Action _onPointerClick;
        public event Action<ExtendedTMP_InputField> OnPointerClickEvent;

        public override void OnPointerClick(PointerEventData eventData)
        {
            OnPointerClickEvent?.Invoke(this);

            base.OnPointerClick(eventData);
        }
    }
}