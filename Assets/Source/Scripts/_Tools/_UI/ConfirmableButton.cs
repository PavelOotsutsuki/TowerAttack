using UnityEngine;
using UnityEngine.EventSystems;

namespace Tools.UI
{
    public class ConfirmableButton : SimpleButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            Image.color = ClickColor;
            CurrentColor = Image.color;
            IsClicked = true;
            CanvasGroup.blocksRaycasts = false;
        }
    }
}