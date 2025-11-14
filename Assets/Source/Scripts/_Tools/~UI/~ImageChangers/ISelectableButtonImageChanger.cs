namespace Tools.UI.ImageChangers
{
    public interface ISelectableButtonImageChanger : IButtonImageChanger
    {
        public void OnPointerClick();
        public void OnEnterClick();
        public void OnExitClick();
    }
}