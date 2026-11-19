using Cards;
using Cards.Views;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerBigCardShowData : IData
    {
        private readonly ReadOnlyTransform _ROTransform;

        public TowerBigCardShowData(Vector2 cardSize, ReadOnlyTransform ROTransform, CardViewData cardViewData)
        {
            CardSize = cardSize;
            CardViewData = cardViewData;

            _ROTransform = ROTransform;
        }

        public Vector2 CardSize { get; private set; }
        public CardViewData CardViewData { get; private set; }

        public float PositionX => _ROTransform.GetPositionX();
    }
}