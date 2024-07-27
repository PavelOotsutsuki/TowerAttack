using Cards;

namespace GameFields.Effects
{
    public abstract class Effect
    {
        public readonly Card Card;
        public readonly EffectTarget Target;

        protected Effect(Card card, EffectTarget target, int turnsCount)
        {
            Card = card;
            Target = target;
            CountTurns = turnsCount;
        }
        
        public int CountTurns { get; private set; }

        public void DecreaseCounter()
        {
            CountTurns -= 1;
        }
    }
}
