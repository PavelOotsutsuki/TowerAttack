namespace Tools
{
    public interface IActivatable
    {
        public void Activate();
    }

    public interface IActivatable<T> where T : IData
    {
        public void Activate(T data);
    }
}