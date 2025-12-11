using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuCardActivateData : IData
    {
        private readonly Vector2 _sizeDelta;
        private readonly CardViewData _cardViewData;

        public LookCardMenuCardActivateData(Vector2 sizeDelta, CardViewData cardViewData)
        {
            _sizeDelta = sizeDelta;
            _cardViewData = cardViewData;
        }

        public float CardHeight => _sizeDelta.y;
        public float CardWidth => _sizeDelta.x;
        public CardViewData CardViewData => _cardViewData;
    }
}