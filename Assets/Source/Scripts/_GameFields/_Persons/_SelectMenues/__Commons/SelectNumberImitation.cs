namespace GameFields.Persons.SelectMenues.Commons
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