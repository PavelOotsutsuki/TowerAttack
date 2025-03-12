namespace GameFields.Persons.Hands
{
    public interface IReadOnlyHand
    {
        public int CountCards { get; }
        public bool IsSlimeEffectCountZero { get; }
    }
}