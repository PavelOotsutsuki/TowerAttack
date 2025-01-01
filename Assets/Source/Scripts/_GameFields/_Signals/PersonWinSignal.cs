using GameFields.Persons;

namespace GameFields.Signals
{
    public struct PersonWinSignal
    {
        public readonly IPersonObject WinnerType;

        public PersonWinSignal(IPersonObject winnerType)
        {
            WinnerType = winnerType;
        }
    }
}