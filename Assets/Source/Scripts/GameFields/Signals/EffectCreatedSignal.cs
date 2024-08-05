using Cards;
using GameFields.Effects;
using GameFields.Persons;

namespace GameFields.Discarding
{
    public struct EffectCreatedSignal
    {
        public readonly Person Target;
        public readonly EffectType Type;

        public EffectCreatedSignal(Person target, EffectType type)
        {
            Type = type;
            Target = target;
        }
    }
}