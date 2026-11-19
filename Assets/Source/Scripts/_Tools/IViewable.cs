namespace Tools
{
    public interface IViewable: IShowable, IHidable
    {
        public bool? IsShown { get; }
    }

    public interface IViewable<T>: IShowable<T>, IHidable where T : IData
    {
        public bool? IsShown { get; }
    }

    public interface IViewable<T, D> : IShowable<T>, IHidable<D> where T : IData where D : IData
    {
        public bool? IsShown { get; }
    }
}