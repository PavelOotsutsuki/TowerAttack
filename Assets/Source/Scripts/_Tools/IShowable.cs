namespace Tools
{
    public interface IShowable
    {
        public void Show();
    }

    public interface IShowable<T> where T: IData
    {
        public void Show(T data);
    }
}