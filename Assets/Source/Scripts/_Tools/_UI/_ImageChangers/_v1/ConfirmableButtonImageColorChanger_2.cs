using UnityEngine;

namespace Tools.UI.ImageChangers.V1
{
    public class ConfirmableButtonImageColorChanger_2 : ButtonImageColorChanger
    {
        public override void OnPointerClick()
        {
            Image.color = ClickColor;
            CurrentColor = Image.color;
        }

        public override void OnEnterClick()
        { }
    }
}