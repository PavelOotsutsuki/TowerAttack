namespace Tools.UI.ImageChangers
{
    public class ConfirmableButtonImageColorChanger : ButtonImageColorChanger, IConfirmableButtonImageChanger
    {
        public void OnPointerClick()
        {
            Image.color = ClickColor;
            CurrentColor = Image.color;
        }

        public override void OnPointerEnter()
        {
            Image.color = SelectColor;
            CurrentColor = SelectColor;
        }

        public override void OnPointerExit()
        {
            CurrentColor = NormalColor;
            Image.color = CurrentColor;
        }
    }
}