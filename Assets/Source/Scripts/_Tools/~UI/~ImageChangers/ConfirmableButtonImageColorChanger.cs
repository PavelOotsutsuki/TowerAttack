namespace Tools.UI.ImageChangers
{
    public class ConfirmableButtonImageColorChanger : ButtonImageColorChanger, IConfirmableButtonImageChanger
    {
        public void OnPointerClick()
        {
            Image.color = ClickColor;
            CurrentColor = Image.color;
        }
    }
}