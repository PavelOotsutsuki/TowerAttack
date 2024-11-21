using GameFields.Persons;

namespace GameFields.Signals
{
    public struct PersonWinSignal
    {
        public readonly Person Winner;

        public PersonWinSignal(Person winner)
        {
            Winner = winner;
        }
    }
}