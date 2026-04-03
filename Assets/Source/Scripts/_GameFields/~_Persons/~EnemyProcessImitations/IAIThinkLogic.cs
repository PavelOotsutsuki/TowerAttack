using Cards;
using Cards.Views;

namespace GameFields.Persons.EnemyProcessImitations
{
    public interface IAIThinkLogic
    {
        public CardCapability FindActionType(Card workCard);
    }
}