using GameFields.Persons;
using GameFields.Persons.CardTransits;

namespace GameFields.StartFights
{
    public abstract class StartTowerCardSelection
    {
        protected readonly ITowerTransitTrySet TowerTransitTrySet;
        private readonly ITowerTransitCheck _towerTransitCheck;

        public bool IsComplete => _towerTransitCheck.IsFill;

        public StartTowerCardSelection(Person person)
        {
            _towerTransitCheck = person;
            TowerTransitTrySet = person;
        }

        public abstract void StartProcess();
    }
}
