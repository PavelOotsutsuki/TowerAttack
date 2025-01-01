namespace GameFields.Persons.AttackMenues
{
    public class AttackResult
    {
        private bool _isAttackSuccess;

        public AttackResult()
        {
            _isAttackSuccess = false;
        }

        public bool IsAttackSuccess => _isAttackSuccess;

        public void SuccessChoice()
        {
            _isAttackSuccess = true;
        }
    }
}