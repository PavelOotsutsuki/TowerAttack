using UnityEngine;

namespace Tools.UI.ImageChangers.V1
{
    public class SelectableButtonImageColorChanger_2 : ButtonImageColorChanger
    {
        public override void OnPointerClick()
        {
            CurrentColor = Image.color;
        }

        public override void OnEnterClick()
        {
            Image.color = ClickColor;
        }

        public void OnExitClick()
        {
            Image.color = NormalColor;
        }
    }
}