namespace GameFields.Persons.Hands
{
    public class HandPlayer : Hand, IPlayerObject
    {
        protected override string GetName() => nameof(HandPlayer);
    }
}