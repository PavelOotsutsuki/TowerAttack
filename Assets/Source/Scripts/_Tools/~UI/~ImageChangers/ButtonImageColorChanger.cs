using UnityEngine;

namespace Tools.UI.ImageChangers
{
    public abstract class ButtonImageColorChanger : ButtonImageChanger, IButtonImageChanger
    {
        [SerializeField] protected Color NormalColor;
        [SerializeField] protected Color ClickColor;
        [SerializeField] protected Color SelectColor;
        [SerializeField] protected Color PressedColor;
        [SerializeField] protected Color DisabledColor;

        protected Color CurrentColor;

        public void OnActivate()
        {
            Image.color = NormalColor;
            CurrentColor = Image.color;
        }

        public void OnPointerDown()
        {
            Image.color = PressedColor;
        }

        public abstract void OnPointerEnter();
        public abstract void OnPointerExit();

        public void OnPointerUp()
        {
            Image.color = CurrentColor;
        }

        public void OnDisabled()
        {
            Image.color = DisabledColor;
        }
    }
}