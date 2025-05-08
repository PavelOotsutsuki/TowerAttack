using UnityEngine.EventSystems;

namespace Tools.UI
{
    public class SelectableButton : SimpleButton
    {
        public sealed override void OnPointerClick(PointerEventData eventData)
        {
            if (IsClicked == false)
            {
                OnEnterClick();
            }
            else
            {
                OnExitClick();
            }

            IsClicked = IsClicked == false;
            CurrentColor = Image.color;
        }

        protected override void OnEnterClick()
        {
            Image.color = ClickColor;
        }

        protected virtual void OnExitClick()
        {
            Image.color = NormalColor;
        }
    }
}