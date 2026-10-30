using Tools;
using UnityEngine;

namespace Cards.Views.BigCardViews.BigCards
{
    public class BigCardShowData : IData
    {
        private readonly ReadOnlyTransform _ROTransform;

        public BigCardShowData(Vector2 cardSize, ReadOnlyTransform ROTransform, CardViewData cardViewData)
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