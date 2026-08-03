using Tools;
using GameFields.Persons.Towers;
using Cards;
using System.Threading;
using GameFields.Persons;

namespace GameFields.StartFights
{
    public abstract class StartTowerCardSelection : ICompletable, IPersonObject
    {
        protected readonly ITowerCardSeatable Tower;

        public bool IsComplete => Tower.HasFreeSeat == false;

        public StartTowerCardSelection(Tower tower)
        {
            Tower = tower;
        }

        public abstract void StartProcess(CancellationToken token);
    }
}