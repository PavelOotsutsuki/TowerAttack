namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberImitation : IAttackNumber
    {
        public AttackNumberImitation(int number)
        {
            Number = number;
        }

        public int Number { get; private set; }
    }
}