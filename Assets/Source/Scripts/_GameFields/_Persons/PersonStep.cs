using Tools.StateMachines;

namespace GameFields.Persons
{
    public abstract class PersonStep : IStateMachineState
    {
        private readonly GameFieldObjectsActivator _gameFieldObjectsActivator;

        public PersonStep(GameFieldObjectsActivator gameFieldObjectsActivator)
        {
            _gameFieldObjectsActivator = gameFieldObjectsActivator;
        }

        public abstract bool IsComplete { get; }

        public void StartStep()
        {
            _gameFieldObjectsActivator.SetObjectsStates(this);

            OnStartStep();
        }

        protected abstract void OnStartStep();
    }
}