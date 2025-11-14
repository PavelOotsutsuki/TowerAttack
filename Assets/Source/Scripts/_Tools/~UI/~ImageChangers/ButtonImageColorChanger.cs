using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.UI.ImageChangers
{
    public abstract class ButtonImageColorChanger : ButtonImageChanger, IButtonImageChanger
    {
        [SerializeField] protected Color NormalColor;
        [SerializeField] protected Color ClickColor;
        [SerializeField] protected Color SelectColor;
        [SerializeField] protected Color PressedColor;

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

        public void OnPointerEnter()
        {
            Image.color = SelectColor;
        }

        public void OnPointerExit()
        {
            Image.color = CurrentColor;
        }

        public void OnPointerUp()
        {
            Image.color = CurrentColor;
        }
    }
}
