using GameFields.Persons;

namespace GameFields.Effects
{
    public abstract class Effect
    {
        public int CountTurns { get; protected set; }

        protected Effect(Person target)
        {
            target.ApplyEffect(this);
        }

        public void DecreaseCounter()
        {
            CountTurns -= 1;
        }
    }
}
