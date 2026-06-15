using System.Threading;
using Tools.StateMachines;

namespace GameFields.Persons
{
    internal abstract class PersonStep : IStateMachineState
    {
        protected readonly CancellationToken Token;

        private readonly InteractionActivator _interactionActivator;

        public PersonStep(InteractionActivator interactionActivator, CancellationToken token)
        {
            _interactionActivator = interactionActivator;
            Token = token;
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