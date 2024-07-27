using GameFields.Persons;

namespace GameFields
{
    public class PersonsState : IPersonsState
    {
        public PersonsState(PersonCreator personCreator)
        {
            ActivePerson = personCreator.CreatePlayer();
            DeactivePerson = personCreator.CreateEnemyAI();
        }

        public Person ActivePerson { get; private set; }
        public Person DeactivePerson { get; private set; }

        public void Switch()
        {
            Person temp = ActivePerson;
            ActivePerson = DeactivePerson;
            DeactivePerson = temp;
        }
    }
}