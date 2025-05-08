namespace GameFields.Persons.Towers
{
    public interface IAttackResultHandler
    {
        void SuccessAttack();
        void FalledAttack();
    }
}