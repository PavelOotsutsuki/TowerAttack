using Tools;

namespace GameFields.Persons.Discovers
{
    public class DiscoverViewLogicData : IData
    {
        private readonly float _cardHeight;
        private readonly float _cardWidth;

        public DiscoverViewLogicData(float cardHeight, float cardWidth)
        {
            _cardHeight = cardHeight;
            _cardWidth = cardWidth;
        }

        public float CardHeight => _cardHeight;
        public float CardWidth => _cardWidth;
    }
}