using Cards;
using GameFields.Effects;

namespace GameFields.Discarding
{
    public struct EffectCreatedSignal
    {
        public readonly CardCharacter Character;
        public readonly Effect Effect;

        public EffectCreatedSignal(CardCharacter character, Effect effect)
        {
            Character = character;
            Effect = effect;
        }
    }
}