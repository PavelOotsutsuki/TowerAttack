using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public class DiscoverCardActivateData: IData
    {
        private Vector2 _sizeDelta;
        private CardViewConfig _cardViewConfig;

        public DiscoverCardActivateData(Vector2 sizeDelta, CardViewConfig cardViewConfig)
        {
            _sizeDelta = sizeDelta;
            _cardViewConfig = cardViewConfig;
        }

        public float CardHeight => _sizeDelta.y;
        public float CardWidth => _sizeDelta.x;
        public CardViewConfig CardViewConfig => _cardViewConfig;
    }
}