using GameFields.Persons;
using GameFields.Persons.CardTransits;
using Tools;

namespace GameFields.StartFights
{
    public abstract class StartTowerCardSelection : ICompletable
    {
        protected readonly ITowerTransitSet TowerTransitSet;
        protected readonly ITowerTransitCheck TowerTransitCheck;

        public bool IsComplete => TowerTransitCheck.IsFill;

        public StartTowerCardSelection(Person person)
        {
            TowerTransitCheck = person;
            TowerTransitSet = person;
        }

        public abstract void StartProcess();
    }
}
