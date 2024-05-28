namespace GameFields
{
    public interface ITurnStep: IStateMachineState
    {
        public void PrepareToStart();
    }
}
