namespace GameFields.Persons
{
    public interface Person_2 : IPlayCardManager, IDrawCardManager
    {
        public int CountDrawCards { get; }
    }
}
