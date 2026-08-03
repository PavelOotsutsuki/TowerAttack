using Tools;

namespace Servers
{
    public class FightProcessDBActionData : IData
    {
        private readonly int _turnNumber;
        private readonly bool? _isPlayersAction;
        private readonly string _action_target;
        private readonly string _action_type;
        private readonly string _action_subtype;

        public FightProcessDBActionData(int turnNumber, bool? isPlayersAction, string action_target, string action_type, string action_subtype)
        {
            _turnNumber = turnNumber;
            _isPlayersAction = isPlayersAction;
            _action_target = action_target;
            _action_type = action_type;
            _action_subtype = action_subtype;
        }

        public int TurnNumber => _turnNumber;
        public bool? IsPlayersAction => _isPlayersAction;
        public string Action_target => _action_target;
        public string Action_type => _action_type;
        public string Action_subtype => _action_subtype;
    }
}