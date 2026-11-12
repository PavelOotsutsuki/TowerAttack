using Tools;
using UnityEngine;

namespace Cards.Views.BigCardViews.BigCards
{
    public class BigCardShowData : IData
    {
        private readonly ReadOnlyRectTransform _RORTransform;

        public BigCardShowData(Vector2 cardSize, ReadOnlyRectTransform RORTransform, CardViewData cardViewData, BigCardPositionType positionType)
        {
            CardSize = cardSize;
            CardViewData = cardViewData;

            _RORTransform = RORTransform;
            PositionType = positionType;
        }

        public Vector2 CardSize { get; private set; }
        public CardViewData CardViewData { get; private set; }
        public BigCardPositionType PositionType { get; private set; }

        public float PositionX => _RORTransform.GetPositionX();
        public float PositionY => _RORTransform.GetPositionY();
        public float Width => _RORTransform.GetWidth();
    }
}