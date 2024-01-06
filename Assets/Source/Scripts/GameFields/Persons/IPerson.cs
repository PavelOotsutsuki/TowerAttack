namespace GameFields.Persons
{
    public interface IPerson : IPlayCardManager, IDrawCardManager
    {
        public float DrawCardsDelay { get; }
        public int CountDrawCards { get; }
    }
}
