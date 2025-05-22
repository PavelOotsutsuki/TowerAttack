using Tools.StateMachines;

namespace GameFields.Persons.Common
{
    public interface ITurnStep : IStateMachineState
    {
        public void FinishTurn();
    }
}