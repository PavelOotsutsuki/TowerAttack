using Tools;
using UnityEngine;

namespace Cards
{
    public class BigCardShowData : IShowableData
    {
        private readonly ReadOnlyTransform _readOnlyTransform;

        public BigCardShowData(Vector2 cardSize, ReadOnlyTransform readOnlyTransform, CardViewConfig cardViewConfig)
        {
            CardSize = cardSize;
            CardViewConfig = cardViewConfig;

            _readOnlyTransform = readOnlyTransform;
        }

        public Vector2 CardSize { get; private set; }
        public CardViewConfig CardViewConfig { get; private set; }

        public float PositionX => _readOnlyTransform.GetPositionX();
    }
}