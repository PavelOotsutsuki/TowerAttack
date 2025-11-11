using Tools;
using UnityEngine;

namespace Cards
{
    public class BigCardShowData : IData
    {
        private readonly ReadOnlyTransform _readOnlyTransform;

        public BigCardShowData(Vector2 cardSize, ReadOnlyTransform readOnlyTransform, CardViewData cardViewData)
        {
            CardSize = cardSize;
            CardViewData = cardViewData;

            _readOnlyTransform = readOnlyTransform;
        }

        public Vector2 CardSize { get; private set; }
        public CardViewData CardViewData { get; private set; }

        public float PositionX => _readOnlyTransform.GetPositionX();
    }
}