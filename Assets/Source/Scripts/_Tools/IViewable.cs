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
}