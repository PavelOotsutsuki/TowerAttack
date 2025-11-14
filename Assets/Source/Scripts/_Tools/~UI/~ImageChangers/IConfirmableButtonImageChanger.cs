namespace Tools.UI.ImageChangers
{
    public interface IConfirmableButtonImageChanger: IButtonImageChanger
    {
        public void OnPointerClick();
    }
}