using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public abstract class HandSeatsSorter
    {
        private readonly float _offsetX;
        private readonly float _handLength;
        private readonly float _startPositionX;
        private readonly float _startPositionY;
        private readonly float _startCardTranslateSpeed;
        private readonly RectTransform _rectTransform;

        public HandSeatsSorter(float offsetX, float handLength, float startPositionX, float startPositionY, float startCardTranslateSpeed, RectTransform rectTransform)
        {
            _offsetX = offsetX;
            _handLength = handLength;
            _startPositionX = startPositionX;
            _startPositionY = startPositionY;
            _startCardTranslateSpeed = startCardTranslateSpeed;
            _rectTransform = rectTransform;
        }

        public float OffsetX => _offsetX;
        public float HandLength => _handLength;
        public float StartPositionX => _startPositionX;
        public float StartPositionY => _startPositionY;
        public float StartCardTranslateSpeed => _startCardTranslateSpeed;
        public RectTransform RectTransform => _rectTransform;

        public abstract void SortHandSeats(List<HandSeat> handSeats);
    }
}
