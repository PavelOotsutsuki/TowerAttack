using Tools.StateMachines;

namespace GameFields.Persons
{
    public interface ITurnStep : IStateMachineState
    {
        public void FinishTurn();
    }
}