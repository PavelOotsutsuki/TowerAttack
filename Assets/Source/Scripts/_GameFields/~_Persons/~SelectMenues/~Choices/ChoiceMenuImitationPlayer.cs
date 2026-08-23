namespace GameFields.Persons.SelectMenues.Choices
{
    public class ChoiceMenuImitationPlayer : SelectMenuImitation, IPlayerObject
    {
        protected override string GetSelectType() => "CHOICE";
    }
}