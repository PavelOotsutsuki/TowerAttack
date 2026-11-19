namespace GameFields.Persons.SelectMenues.Choices
{
    public class ChoiceMenuImitationEnemyAI : SelectMenuImitation, IEnemyAIObject
    {
        protected override string GetSelectType() => "CHOICE";
    }
}