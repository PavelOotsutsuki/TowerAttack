using Tools;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberStateViewActivateData : IData
    {
        private readonly bool _isActiveView;

        public AttackNumberStateViewActivateData(bool isActiveView)
        {
            _isActiveView = isActiveView;
        }

        public bool IsActiveView => _isActiveView;
    }
}