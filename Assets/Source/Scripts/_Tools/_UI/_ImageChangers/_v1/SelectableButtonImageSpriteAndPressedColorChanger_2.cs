using UnityEngine;

namespace Tools.UI.ImageChangers.V1
{
    public class SelectableButtonImageSpriteAndPressedColorChanger_2 : ButtonImageSpriteAndPressedColorChanger
    {
        public override void OnPointerClick()
        {
            CurrentSprite = Image.sprite;
        }

        public override void OnEnterClick()
        {
            Image.color = NormalColor;
            Image.sprite = ClickSprite;
        }

        public void OnExitClick()
        {
            Image.color = NormalColor;
            Image.sprite = NormalSprite;
        }
    }
}