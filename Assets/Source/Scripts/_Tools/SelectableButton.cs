using UnityEngine.EventSystems;

namespace Tools
{
    public class SelectableButton : SimpleButton
    {
        public override void OnPointerClick(PointerEventData eventData)
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

        protected virtual void OnEnterClick()
        {
            Image.color = ClickColor;
        }

        protected virtual void OnExitClick()
        {
            Image.color = NormalColor;
        }
    }
}