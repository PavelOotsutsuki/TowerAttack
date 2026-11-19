using System;
using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils.Movements;
using Tools.Utils.Screens;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Towers
{
    public class AttackAnimation: ICompletable
    {
        private readonly Movement _cardMovement;
        private readonly ReadOnlyRectTransform _cardTransform;
        private readonly Vector2 _towerLocalPosition;
        private readonly Vector2 _towerSize;
        private readonly AttackAnimationData _data;

        private float _xPeekToFirstCardPositionOffset;
        private float _yPeekToFirstCardPositionOffset;
        private float _xOffset;
        private float _yOffset;

        private float _cardAngle;

        public AttackAnimation(Movement cardMovement, ReadOnlyRectTransform cardTransform, Vector3 towerLocalPosition
            , Vector2 towerSize, AttackAnimationData data)
        {
            _cardMovement = cardMovement;
            _cardTransform = cardTransform;
            _towerLocalPosition = towerLocalPosition;
            _towerSize = towerSize;
            _data = data;
        }

        public bool IsComplete { get; private set; }

        private Vector2 CardSize => _cardTransform.GetRect();
        private Vector2 CardLocalPosition => _cardTransform.GetLocalPosition();
        private Vector3 CardScale => _cardTransform.GetLocalScale();
        private Vector3 CardRotation => _cardTransform.GetRotationVector();

        private float DownOrUpVector => Convert.ToInt32(_data.IsDown) * 2 - 1;
        private float DownOrUpRotation => _data.IsDown ? 0f : 180f;

        public void Play(CancellationToken token)
        {
            IsComplete = false;

            Playing(token).Forget();
        }

        private async UniTask Playing(CancellationToken token)
        {
            Vector2 atTheReadyLocalPosition = FindAtTheReadyPosition();
            Vector3 rotation = FindAtTheReadyRotation();

            _cardMovement.MoveLocalSmoothly(atTheReadyLocalPosition, rotation, _data.AtTheReadyMoveDuration, CardScale);
            await UniTask.WaitForSeconds(_data.AtTheReadyMoveDuration + _data.AfterAtTheReadyMoveDelay, cancellationToken: token);

            Vector2 endLocalPosition = FindEndPosition();

            _cardMovement.MoveLocalInOutBack(endLocalPosition, rotation, _data.EndMoveDuration, CardScale);
            await UniTask.WaitForSeconds(_data.EndMoveDuration * _data.InOutBackFactor, cancellationToken: token);

            IsComplete = true;
        }

        private Vector2 FindAtTheReadyPosition()
        {
            _xPeekToFirstCardPositionOffset = _towerSize.x / 2 + CardSize.x / 2 + CardSize.x * _data.OffsetXCardFactor;
            _yPeekToFirstCardPositionOffset = _towerSize.y / 2 + CardSize.y / 2 + CardSize.y * _data.OffsetYCardFactor;

            _xOffset = _xPeekToFirstCardPositionOffset;
            _yOffset = _yPeekToFirstCardPositionOffset;

            bool isFullX = Convert.ToBoolean(Random.Range(0, 2));

            if (isFullX)
            {
                _yOffset = Random.Range(0, _yOffset);
            }
            else
            {
                _xOffset = Random.Range(0, _xOffset);
            }

            //return new Vector2(_towerPosition.x + _xOffset, _towerPosition.y - _yOffset);
            return new Vector2(_towerLocalPosition.x + _xOffset, _towerLocalPosition.y - _yOffset * DownOrUpVector);
            //return new Vector2(_towerPosition.x + _xOffset, _towerPosition.y + _yOffset);
        }

        private Vector3 FindAtTheReadyRotation()
        {
            Vector2 currentRotation = CardRotation;

            if (_xOffset > _towerSize.x / 2 && _yOffset > _towerSize.y / 2)
            {
                //_cardAngle = Mathf.Atan((_xOffset - _towerSize.x / 2) / (_yOffset - _towerSize.y / 2)) * 180 / Mathf.PI;
                _cardAngle = DownOrUpRotation + DownOrUpVector * (Mathf.Atan((_xOffset - _towerSize.x / 2) / (_yOffset - _towerSize.y / 2)) * 180 / Mathf.PI);
                //_cardAngle = 180f - (Mathf.Atan((_xOffset - _towerSize.x / 2) / (_yOffset - _towerSize.y / 2)) * 180 / Mathf.PI);
            }
            else
            {
                if (Mathf.Approximately(_xPeekToFirstCardPositionOffset, _xOffset))
                {
                    _cardAngle = 90f;
                }
                else if (Mathf.Approximately(_yPeekToFirstCardPositionOffset, _yOffset))
                {
                    //_cardAngle = 0f;
                    _cardAngle = DownOrUpRotation;
                    //_cardAngle = 180f;
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
                return new Vector2(_towerLocalPosition.x + _towerSize.x / 2, CardLocalPosition.y);
            }
            //else if (_cardAngle == 0f)
            else if (_cardAngle == DownOrUpRotation)
            //else if (_cardAngle == 180f)
            {
                //return new Vector2(CardPosition.x, _towerPosition.y - _towerSize.y / 2);
                return new Vector2(CardLocalPosition.x, _towerLocalPosition.y - DownOrUpVector * _towerSize.y / 2);
                //return new Vector2(CardPosition.x, _towerPosition.y + _towerSize.y / 2);
            }
            else
            {
                //return new Vector2(_towerPosition.x + _towerSize.x / 2, _towerPosition.y - _towerSize.y / 2);
                return new Vector2(_towerLocalPosition.x + _towerSize.x / 2, _towerLocalPosition.y - DownOrUpVector * _towerSize.y / 2);
                //return new Vector2(_towerPosition.x + _towerSize.x / 2, _towerPosition.y + _towerSize.y / 2);
            }
        }
    }
}