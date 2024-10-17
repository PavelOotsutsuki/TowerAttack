namespace Tools
{
    public interface IStateMachineState : ICompletable
    {
        public void StartStep();
    }
}
