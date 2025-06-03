using Tools;
using GameFields.Persons.Towers;
using Cards;

namespace GameFields.StartFights
{
    public abstract class StartTowerCardSelection : ICompletable
    {
        protected readonly ICardDropPlace Tower;

        public bool IsComplete => Tower.HasFreeSeat == false;

        public StartTowerCardSelection(Tower tower)
        {
            Tower = tower;
        }

        public abstract void StartProcess();
    }
}