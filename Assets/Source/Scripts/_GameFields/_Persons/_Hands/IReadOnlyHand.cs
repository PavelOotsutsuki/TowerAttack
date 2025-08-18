namespace GameFields.Persons.Hands
{
    public interface IReadOnlyHand
    {
        public int CountHandSeats { get; }
        public bool IsSlimeEffectCountZero { get; }
    }
}