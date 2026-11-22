using Cards;
using Cards.Effects;
using GameFields.Effects;

namespace GameFields.Persons
{
    public interface IReadOnlyPersonEffect
    {
        public Card Card { get; }
        public ICardEffectConfig CardEffectConfig { get; }
        public Effect Effect { get; }
    }
}
