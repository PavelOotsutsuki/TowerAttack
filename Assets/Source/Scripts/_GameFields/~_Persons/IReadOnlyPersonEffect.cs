using Cards;
using Cards.Effects;
using GameFields.Effects;

namespace GameFields.Persons
{
    public interface IReadOnlyPersonEffect
    {
        public Card Card { get; }
        public CardEffectConfig CardEffectConfig { get; }
        public Effect Effect { get; }
    }
}
