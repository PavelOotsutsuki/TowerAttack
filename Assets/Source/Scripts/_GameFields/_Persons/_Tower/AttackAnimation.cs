using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils.Movements;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Towers
{
    public class AttackAnimation
    {
        private readonly Movement _cardMovement;
        private readonly ReadOnlyRectTransform _cardTransform;
        private readonly Vector2 _towerPosition;
        private readonly Vector2 _towerSize;
        private readonly AttackAnimationData _data;

        private float _xPeekToFirstCardPositionOffset;
        private float _yPeekToFirstCardPositionOffset;
        private float _xOffset;
        private float _yOffset;

        private float _cardAngle;
        private float _translateFactor;

        public AttackAnimation(Movement cardMovement, ReadOnlyRectTransform cardTransform, Vector3 towerPosition
            , Vector2 towerSize, AttackAnimationData data)
        {
            _cardMovement = cardMovement;
            _cardTransform = cardTransform;
            _towerPosition = towerPosition;
            _towerSize = towerSize;
            _data = data;
        }

        private Vector2 CardSize => _cardTransform.GetRect();
        private Vector2 CardPosition => _cardTransform.GetPosition();
        private Vector3 CardScale => _cardTransform.GetLocalScale();
        private Vector3 CardRotation => _cardTransform.GetRotationVector();

        public void Play()
        {
            Playing().ToUniTask();
        }

        private IEnumerator Playing()
        {
            Vector2 atTheReadyPosition = FindAtTheReadyPosition();
            Vector3 rotation = FindAtTheReadyRotation();

            _cardMovement.MoveSmoothly(atTheReadyPosition, rotation, 1f, CardScale);

            yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(0.5f);

            //Vector2 backPosition = FindBackPosition();

            //_currentCardMovement.MoveLinear(backPosition, firstRotation, 2f, _currentCardTransform.GetLocalScale());

            //yield return new WaitForSeconds(2f);

            //Vector2 endPosition = FindEndPosition();

            //_currentCardMovement.MoveLinear(endPosition, firstRotation, 0.5f, _currentCardTransform.GetLocalScale());

            Vector2 endPosition = FindEndPosition();
            _cardMovement.MoveInOutBack(endPosition, rotation, 1f, CardScale);
            yield return new WaitForSeconds(0.6f);
        }

        private Vector2 FindAtTheReadyPosition()
        {
            _xPeekToFirstCardPositionOffset = _towerSize.x / 2 + CardSize.x / 2 + CardSize.x * _data.OffsetXCardFactor;
            _yPeekToFirstCardPositionOffset = _towerSize.y / 2 + CardSize.y / 2 + CardSize.y * _data.OffsetYCardFactor;

            _xOffset = _xPeekToFirstCardPositionOffset;
            _yOffset = _yPeekToFirstCardPositionOffset;

            bool isFullX = Convert.ToBoolean(Random.Range(0, 2));
            //_xSide = Random.Range(0, 2) * 2 - 1;

            if (isFullX)
            {
                _yOffset = Random.Range(0, _yOffset);
            }
            else
            {
                _xOffset = Random.Range(0, _xOffset);
            }

            //_xOffset *= _xSide;


            return new Vector2(_towerPosition.x + _xOffset, _towerPosition.y - _yOffset);
        }

        private Vector3 FindAtTheReadyRotation()
        {
            Vector2 currentRotation = CardRotation;


            //if (_xOffset - CardSize.x/2 > _towerSize.x/2 && _yOffset - CardSize.x/2 > _towerSize.y/2)
            if (_xOffset > _towerSize.x / 2 && _yOffset > _towerSize.y / 2)
            {
                _cardAngle = Mathf.Atan((_xOffset - _towerSize.x / 2) / (_yOffset - _towerSize.y / 2)) * 180 / Mathf.PI;
            }
            else
            {
                if (Mathf.Approximately(_xPeekToFirstCardPositionOffset, _xOffset))
                {
                    _cardAngle = 90f;
                }
                else if (Mathf.Approximately(_yPeekToFirstCardPositionOffset, _yOffset))
                {
                    _cardAngle = 0f;
                }
                else
                {
                    throw new Exception("Ни одна сторона не идет по максимуму");
                }
            }

            Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y, _cardAngle);

            return newRotation;
        }

        private Vector2 FindEndPosition()
        {
            if (_cardAngle == 90f)
            {
                return new Vector2(_towerPosition.x + _towerSize.x / 2, CardPosition.y);
            }
            else if (_cardAngle == 0f)
            {
                return new Vector2(CardPosition.x, _towerPosition.y - _towerSize.y / 2);
            }
            else
            {
                return new Vector2(_towerPosition.x + _towerSize.x / 2, _towerPosition.y - _towerSize.y / 2);
            }
        }
    }
}
