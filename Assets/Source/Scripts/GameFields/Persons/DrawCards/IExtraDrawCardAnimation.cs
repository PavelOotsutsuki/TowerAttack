namespace GameFields.Persons.DrawCards
{
    public interface IExtraDrawCardAnimation : IDrawCardAnimation
    {
        public int ActiveTurnsCount { get; }
    }
}
