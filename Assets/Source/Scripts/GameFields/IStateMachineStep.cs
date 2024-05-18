namespace GameFields
{
    public interface IStateMachineStep
    {
        public bool IsComplete { get; }

        //public void PrepareToStart();
        public void StartStep();
    }
}
