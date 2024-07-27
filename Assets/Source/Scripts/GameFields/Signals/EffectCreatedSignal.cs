using GameFields.Effects;
using GameFields.Persons;

namespace GameFields.Discarding
{
    public struct EffectCreatedSignal
    {
        public readonly Person Target;
        public readonly Effect Effect;

        public EffectCreatedSignal(Person target, Effect effect)
        {
            Effect = effect;
            Target = target;
        }
    }
}