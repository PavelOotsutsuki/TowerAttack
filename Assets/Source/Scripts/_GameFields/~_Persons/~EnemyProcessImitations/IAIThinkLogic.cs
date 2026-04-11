using Cards;

namespace GameFields.Persons.EnemyProcessImitations
{
    public interface IAIThinkLogic
    {
        public CapabilityProbability FindActionType(Card workCard);
    }
}