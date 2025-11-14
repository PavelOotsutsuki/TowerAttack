namespace Tools.UI.ImageChangers
{
    public interface IButtonImageChanger
    {
        public void OnActivate();
        //public void OnEnterClick();
        //public void OnPointerClick();
        public void OnPointerEnter();
        public void OnPointerDown();
        public void OnPointerExit();
        public void OnPointerUp();
    }
}
