namespace Cards
{
    public interface ICardPlayPlace
    {
        public bool TryPlayCard(IPlayable playableObject);
    }
}
