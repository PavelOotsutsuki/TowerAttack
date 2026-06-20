namespace Tools
{
    public interface IReactivatable
    {
        public void Reactivate();
    }

    public interface IReactivatable<R> where R : IData
    {
        public void Reactivate(R data);
    }
}