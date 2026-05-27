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

        public override void OnPointerEnter()
        {
            Image.color = NormalColor;
            Image.sprite = SelectSprite;
            CurrentSprite = SelectSprite;
        }

        public override void OnPointerExit()
        {
            CurrentSprite = NormalSprite;
            Image.color = NormalColor;
            Image.sprite = CurrentSprite;
        }
    }
}