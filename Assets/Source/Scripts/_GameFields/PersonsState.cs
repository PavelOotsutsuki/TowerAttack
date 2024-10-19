using GameFields.Persons;

namespace GameFields
{
    public class PersonsState : IPersonsState
    {
        public PersonsState(Person active, Person  deactive)
        {
            Active = active;
            Deactive = deactive;
        }

        public Person Active { get; private set; }
        public Person Deactive { get; private set; }

        public void Switch()
        {
            Person temp = Active;
            Active = Deactive;
            Deactive = temp;
        }
    }
}