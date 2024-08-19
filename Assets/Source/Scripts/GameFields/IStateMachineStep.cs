namespace GameFields
{
    public interface IStateMachineState
    {
        public bool IsComplete { get; }

        //public void PrepareToStart();
        public void StartStep();
    }
}
