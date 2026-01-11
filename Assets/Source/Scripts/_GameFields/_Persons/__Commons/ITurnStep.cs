using Tools.StateMachines;

namespace GameFields.Persons.Commons
{
    public interface ITurnStep : IStateMachineState
    {
        public void FinishTurn();
    }
}