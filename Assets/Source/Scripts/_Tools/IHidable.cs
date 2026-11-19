namespace Tools
{
    public interface IHidable
    {
        public void Hide();
    }

    public interface IHidable<D> where D : IData
    {
        public void Hide(D data);
    }
}