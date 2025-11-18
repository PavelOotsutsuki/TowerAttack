namespace Tools
{
    public interface IWorkable: IActivatable, IDeactivatable
    {
        public bool? IsActive { get; }
    }

    public interface IWorkable<T> : IActivatable<T>, IDeactivatable where T : IData
    {
        public bool? IsActive { get; }
    }
}