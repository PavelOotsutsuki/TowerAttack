namespace GameFields.Persons.Towers
{
    public interface IAttackResultHandler: ICardNumberKeeper
    {
        void SuccessAttack();
    }
}