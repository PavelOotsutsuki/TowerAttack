using UnityEngine.EventSystems;

namespace Tools
{
    public class ConfirmableButton : SimpleButton
    {
        public override void OnPointerClick(PointerEventData eventData)
        {
            Image.color = ClickColor;
            CurrentColor = Image.color;
            IsClicked = true;
        }
    }
}