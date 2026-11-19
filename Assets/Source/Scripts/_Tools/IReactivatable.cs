namespace Tools
{
    public interface IReactivatable : IActivatable // Интерфейс для переактивации класса (без деактивации), но логика другая
    {
        public void Reactivate();
    }

    public interface IReactivatable<R> : IActivatable where R : IData
    {
        public void Reactivate(R data);
    }
}