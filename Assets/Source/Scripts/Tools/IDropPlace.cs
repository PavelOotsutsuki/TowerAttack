namespace Tools
{
    public interface IDropPlace
    {
        public bool TrySeat<T>(T seatable) where T: class;
    }
}