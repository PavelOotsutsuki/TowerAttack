using Tools;
using Tools.UI;
using UnityEngine;

namespace Cards
{
    public class BigCardDescriptionActivateData : IData
    {
        //private readonly float _bigCardWidth;
        //private readonly Vector2 _bigCardPosition;
        //private readonly Vector2 _bigCardSize;
        private readonly LabelActivateData _labelActivateData;

        public BigCardDescriptionActivateData(//float bigCardWidth, Vector2 bigCardPosition, Vector2 bigCardSize,
            LabelActivateData labelActivateData)
        {
            //_bigCardWidth = bigCardWidth;
            //_bigCardPosition = bigCardPosition;
            //_bigCardSize = bigCardSize;
            _labelActivateData = labelActivateData;
        }

        //public float BigCardWidth => _bigCardWidth;
        //public Vector2 BigCardPosition => _bigCardPosition;
        //public Vector2 BigCardSize => _bigCardSize;
        public LabelActivateData LabelActivateData => _labelActivateData;
    }
}