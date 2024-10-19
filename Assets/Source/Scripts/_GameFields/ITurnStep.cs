using Tools.StateMachines;

namespace GameFields
{
    public interface ITurnStep : IStateMachineState
    {
        public void FinishTurn();
    }
}