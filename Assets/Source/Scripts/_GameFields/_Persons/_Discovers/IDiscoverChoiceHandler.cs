using Cards;

namespace GameFields.Persons.Discovers
{
    public interface IDiscoverChoiceHandler
    {
        public void OnMakeChoice(IDiscoverable card);
    }
}