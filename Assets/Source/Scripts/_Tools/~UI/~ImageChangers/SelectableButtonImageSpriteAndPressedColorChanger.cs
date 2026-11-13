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

        public override void OnPointerEnter()
        {
            Image.color = NormalColor;
            Image.sprite = SelectSprite;
        }

        public override void OnPointerExit()
        {
            Image.color = NormalColor;
            Image.sprite = CurrentSprite;
        }
    }
}