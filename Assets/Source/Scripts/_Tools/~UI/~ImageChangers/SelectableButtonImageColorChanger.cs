namespace Tools.UI.ImageChangers
{
    public class SelectableButtonImageColorChanger : ButtonImageColorChanger, ISelectableButtonImageChanger
    {
        public void OnPointerClick()
        {
            CurrentColor = Image.color;
        }

        public void OnEnterClick()
        {
            Image.color = ClickColor;
        }

        public void OnExitClick()
        {
            Image.color = NormalColor;
        }

        public override void OnPointerEnter()
        {
            Image.color = SelectColor;
        }

        public override void OnPointerExit()
        {
            Image.color = CurrentColor;
        }
    }
}