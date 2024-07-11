using Cards;

namespace GameFields.Discarding
{
    public struct CardPlayedSignal
    {
        public readonly CardCharacter Character;

        public CardPlayedSignal(CardCharacter character)
        {
            Character = character;
        }
    }
}