namespace Tools.UI.ImageChangers
{
    public class ConfirmableButtonImageSpriteAndPressedColorChanger : ButtonImageSpriteAndPressedColorChanger, IConfirmableButtonImageChanger
    {
        public void OnPointerClick()
        {
            Image.color = NormalColor;
            Image.sprite = ClickSprite;
            CurrentSprite = Image.sprite;
        }
    }
}