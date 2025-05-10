using UnityEngine;

namespace Tools.UI.ImageChangers.V1
{
    public abstract class ButtonImageColorChanger : ButtonImageChangerRealization
    {
        [SerializeField] protected Color NormalColor;
        [SerializeField] protected Color ClickColor;
        [SerializeField] protected Color SelectColor;
        [SerializeField] protected Color PressedColor;

        protected Color CurrentColor;

        public override void OnActivate()
        {
            Image.color = NormalColor;
            CurrentColor = Image.color;
        }

        public override void OnPointerDown()
        {
            Image.color = PressedColor;
        }

        public override void OnPointerEnter()
        {
            Image.color = SelectColor;
        }

        public override void OnPointerExit()
        {
            Image.color = CurrentColor;
        }

        public override void OnPointerUp()
        {
            Image.color = CurrentColor;
        }
    }
}