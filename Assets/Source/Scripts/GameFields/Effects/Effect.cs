namespace GameFields.Effects
{
    public abstract class Effect
    {
        public int CountTurns { get; protected set; }

        public void DecreaseCounter()
        {
            CountTurns -= 1;
        }
    }
}
