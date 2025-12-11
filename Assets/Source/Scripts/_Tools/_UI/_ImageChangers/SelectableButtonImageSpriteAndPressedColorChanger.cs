namespace Tools.UI.ImageChangers
{
    public class SelectableButtonImageSpriteAndPressedColorChanger : ButtonImageSpriteAndPressedColorChanger, ISelectableButtonImageChanger
    {
        public void OnPointerClick()
        {
            CurrentSprite = Image.sprite;
        }

        public void OnEnterClick()
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