namespace Tools
{
    public interface IShowable
    {
        public void Show();
    }

    public interface IShowable<T> where T: IShowableData
    {
        public void Show(T data);
    }
}