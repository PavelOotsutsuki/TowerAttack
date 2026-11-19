using Tools;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuCardViewLogicData : IData
    {
        private readonly float _cardHeight;
        private readonly float _cardWidth;

        public LookCardMenuCardViewLogicData(float cardHeight, float cardWidth)
        {
            _cardHeight = cardHeight;
            _cardWidth = cardWidth;
        }

        public float CardHeight => _cardHeight;
        public float CardWidth => _cardWidth;
    }
}