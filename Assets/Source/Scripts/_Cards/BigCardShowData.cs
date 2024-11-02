using Tools;
using Tools.UI.Fadings;
using UnityEngine;

namespace Cards
{
    public class BigCardShowData : IData
    {
        private readonly ReadOnlyTransform _readOnlyTransform;

        public BigCardShowData(Vector2 cardSize, ReadOnlyTransform readOnlyTransform, CardViewConfig cardViewConfig)
        {
            CardSize = cardSize;
            CardViewConfig = cardViewConfig;
            LabelData = new FadableLabelActivateData(CardViewConfig.Description);

            _readOnlyTransform = readOnlyTransform;
        }

        public Vector2 CardSize { get; private set; }
        public CardViewConfig CardViewConfig { get; private set; }
        public FadableLabelActivateData LabelData { get; private set; }

        public float PositionX => _readOnlyTransform.GetPositionX();
    }
}