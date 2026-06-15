namespace Tools
{
    public interface IDeactivatable
    {
        public void Deactivate();
    }

    public interface IDeactivatable<D> where D : IData
    {
        public void Deactivate(D data);
    }
}