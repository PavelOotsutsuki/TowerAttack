using UnityEngine;

namespace Tools.UI.ImageChangers.V1
{
    public class ConfirmableButtonImageSpriteAndPressedColorChanger_2 : ButtonImageSpriteAndPressedColorChanger
    {
        public override void OnPointerClick()
        {
            Image.color = NormalColor;
            Image.sprite = ClickSprite;
            CurrentSprite = Image.sprite;
        }

        public override void OnEnterClick()
        { }
    }
}