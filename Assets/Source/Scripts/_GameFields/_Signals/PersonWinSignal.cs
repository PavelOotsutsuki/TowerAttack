using GameFields.Persons;

namespace GameFields.Signals
{
    public struct PersonWinSignal
    {
        public readonly IPersonObject TowerAttackedType;

        public PersonWinSignal(IPersonObject winnerType)
        {
            TowerAttackedType = winnerType;
        }
    }
}