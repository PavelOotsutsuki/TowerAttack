using GameFields.Persons;

namespace GameFields.Signals
{
    public struct PersonWinSignal
    {
        public readonly IPersonObject Loser;

        public PersonWinSignal(IPersonObject loserType)
        {
            Loser = loserType;
        }
    }
}