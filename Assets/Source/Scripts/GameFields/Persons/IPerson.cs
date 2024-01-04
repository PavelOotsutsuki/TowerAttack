namespace GameFields.Persons
{
    public interface IPerson : IPlayCardManager, IDrawCardManager
    {
        public int CountDrawCards { get; }
    }
}
