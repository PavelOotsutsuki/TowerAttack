using Tools;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberAnimationActivateData : IData
    {
        private readonly bool _isActiveView;

        public AttackNumberAnimationActivateData(bool isActiveView)
        {
            _isActiveView = isActiveView;
        }

        public bool IsActiveView => _isActiveView;
    }
}