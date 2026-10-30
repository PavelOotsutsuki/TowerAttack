namespace Cards.DependencyInterlayers
{
    public interface ITableDrop : ICardDropPlace
    {
        public bool CanPlay { get; }
    }
}