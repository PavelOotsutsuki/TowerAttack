using Cards;

namespace GameFields.Persons.DrawCards
{
    public interface IDrawCardAnimation
    {
        public bool IsDone { get; }

        public void Play(Card card);
    }
}