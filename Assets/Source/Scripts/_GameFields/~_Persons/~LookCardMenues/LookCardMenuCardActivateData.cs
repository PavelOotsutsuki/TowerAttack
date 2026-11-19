using System.Threading;
using Cards;
using Cards.Views;
using Tools;
using UnityEngine;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuCardActivateData : CancellationTokenData
    {
        private readonly Vector2 _sizeDelta;
        private readonly CardViewData _cardViewData;

        public LookCardMenuCardActivateData(Vector2 sizeDelta, CardViewData cardViewData, CancellationToken cardToken) : base(cardToken)
        {
            _sizeDelta = sizeDelta;
            _cardViewData = cardViewData;
        }

        public float CardHeight => _sizeDelta.y;
        public float CardWidth => _sizeDelta.x;
        public CardViewData CardViewData => _cardViewData;
    }
}