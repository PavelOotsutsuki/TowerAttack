using GameFields.InputSettings;
using Tools;

namespace GameFields.EndTurnButtons
{
    public class EndTurnButtonInputLogic : IInputLogic, IEnterPressHandler
    {
        private readonly IDeactivatable _endTurnButtonDeactivatable;

        public EndTurnButtonInputLogic(IDeactivatable endTurnButtonDeactivatable)
        {
            _endTurnButtonDeactivatable = endTurnButtonDeactivatable;
        }

        public void OnEnter()
        {
            _endTurnButtonDeactivatable.Deactivate();
        }
    }
}