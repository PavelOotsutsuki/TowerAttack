using Tools.StateMachines;

namespace GameFields.Persons
{
    public abstract class PersonStep : IStateMachineState
    {
        private readonly InteractionActivator _interactionActivator;

        public PersonStep(InteractionActivator interactionActivator)
        {
            _interactionActivator = interactionActivator;
        }

        public abstract bool IsComplete { get; }

        public void StartStep()
        {
            _interactionActivator.SetObjectsStates(this);

            OnStartStep();
        }

        protected abstract void OnStartStep();
    }
}