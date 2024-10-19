namespace Tools
{
    public interface IViewable: IShowable, IHidable
    { }

    public interface IViewable<T>: IShowable<T>, IHidable where T : IShowableData
    { }
}