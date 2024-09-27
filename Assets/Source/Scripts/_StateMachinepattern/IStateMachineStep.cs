namespace StateMachine
{
    public interface IStateMachineState
    {
        public bool IsComplete { get; }

        public void StartStep();
    }
}
