namespace GameFields.Persons.SelectMenues
{
    public class SelectNumberImitation : ISelectNumber
    {
        public SelectNumberImitation(int number)
        {
            Number = number;
        }

        public int Number { get; private set; }
    }
}