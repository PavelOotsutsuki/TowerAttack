namespace Tools
{
    public interface IWorkable: IActivatable, IDeactivatable
    { }

    public interface IWorkable<T> : IActivatable<T>, IDeactivatable where T : IData
    { }
}