namespace Tools.StateMachines
{
    public interface IStateMachineState : ICompletable
    {
        public void StartStep();
    }
}