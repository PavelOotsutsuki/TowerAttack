using GameFields.Persons;
using GameFields.Persons.CardTransits;

namespace GameFields.StartFights
{
    public abstract class StartTowerCardSelection
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
