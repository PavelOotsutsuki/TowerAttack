namespace GameFields.Persons.SelectMenues.Attacks
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