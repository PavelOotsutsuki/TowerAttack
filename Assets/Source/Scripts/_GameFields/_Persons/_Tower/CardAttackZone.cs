using System;
using System.Collections;
using Cards;
using GameFields.DiscardPiles;
using GameFields.Persons.AttackMenues;
using Tools;
using Tools.Utils.Movements;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

namespace GameFields.Persons.Towers
{
    public class CardAttackZone : MonoBehaviour, IAttackable
    {
        [SerializeField] private float _offsetXFactor = 0.35f;
        [SerializeField] private float _offsetYFactor = 0.35f;
        [SerializeField] private float _backTranslate = 50f;
        [SerializeField] private Transform _cameraTransform;

        private AttackMenu _attackMenu;
        private ReadOnlyRectTransform _towerTransform;
        private DiscardPile _discardPile;

        private Card _card;
        private Movement _cardMovement;
        private ReadOnlyRectTransform _cardTransform;
        private float _xPeekToFirstCardPositionOffset;
        private float _yPeekToFirstCardPositionOffset;
        private float _xOffset;
        private float _yOffset;
        //private int _xSide;
        private float _cardAngle;
        private float _translateFactor;

        private Vector2 _towerPosition;
        private Vector2 _towerSize;

        private Vector2 CardSize => _cardTransform.GetRect();
        private Vector2 CardPosition => _cardTransform.GetPosition();

        [Inject]
        public void Construct(DiscardPile discardPile)
        {
            _discardPile = discardPile;
        }

        public void Init(AttackMenu attackMenu, IReadOnlyRectTransformable tower)
        {
            _attackMenu = attackMenu;
            _towerTransform = tower.ReadOnlyRectTransform;

            _towerPosition = _towerTransform.GetPosition();
            _towerSize = _towerTransform.GetRect();
        }

        public void Attack(Card card)
        {
            _card = card;
            _cardMovement = _card.CardMovement;
            _cardTransform = _card.ReadOnlyRectTransform;

            StartCoroutine(ActivatingAttack());
        }

        private IEnumerator ActivatingAttack()
        {
            Vector2 firstPosition = FindFirstPosition();
            Vector3 firstRotation = FindFirstRotation();

            _cardMovement.MoveSmoothly(firstPosition, firstRotation, 1f, _cardTransform.GetLocalScale());

            yield return new WaitForSeconds(1f);
            yield return new WaitForSeconds(0.5f);

            //Vector2 backPosition = FindBackPosition();

            //_currentCardMovement.MoveLinear(backPosition, firstRotation, 2f, _currentCardTransform.GetLocalScale());

            //yield return new WaitForSeconds(2f);

            //Vector2 endPosition = FindEndPosition();

            //_currentCardMovement.MoveLinear(endPosition, firstRotation, 0.5f, _currentCardTransform.GetLocalScale());

            Vector2 endPosition = FindEndPosition();
            _cardMovement.MoveInOutBack(endPosition, firstRotation, 1f, _cardTransform.GetLocalScale());
            yield return new WaitForSeconds(0.6f);

            StartCoroutine(ShakeCamera());

            _attackMenu.Activate();

            yield return new WaitUntil(() => _attackMenu.IsComplete);

            StartCoroutine(Discarding());
        }

        private IEnumerator Discarding()
        {
            Card card = _card;

            InvertCardFront(card);
            yield return new WaitForSeconds(0.5f);

            card.SetSide(SideType.Back);

            InvertCardBack(card);
            yield return new WaitForSeconds(0.5f + 1f);

            _discardPile.SeatCard(card);
        }

        private void InvertCardFront(Card card)
        {
            Vector3 position = card.ReadOnlyRectTransform.GetPosition();

            Movement cardMovement = card.CardMovement;

            cardMovement.MoveLinear(position, new Vector3(0f, -90f, 0f), 0.5f);
        }

        private void InvertCardBack(Card card)
        {
            Vector3 endRotationVector = Vector3.zero;
            Vector3 position = card.ReadOnlyRectTransform.GetPosition();

            Movement cardMovement = card.CardMovement;

            cardMovement.MoveSmoothly(position, endRotationVector, 0.5f, card.ReadOnlyRectTransform.GetLocalScale());
        }

        private IEnumerator ShakeCamera()
        {
            float duration = 0.2f;
            Vector3 originalPosition = _cameraTransform.position;

            float x;
            float y;
            float timeLeft = Time.time;

            while ((timeLeft + duration) > Time.time)
            {
                x = Random.Range(-0.3f, 0.3f);
                y = Random.Range(-0.3f, 0.3f);

                _cameraTransform.position = new Vector3(x, y, originalPosition.z);
                yield return new WaitForSeconds(0.025f);
            }

            _cameraTransform.position = originalPosition;
        }

        //private Vector2 FindFirstCoordinates()
        //{
        //    Vector2 towerRightDownAngle = _towerTransform.GetRightDownAnglePosition();
        //    Vector2 cardSize = _currentCardTransform.GetSizeDelta();

        //    return new Vector2(towerRightDownAngle.x + cardSize.x / 2, towerRightDownAngle.y - cardSize.y / 2);
        //}
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

        private Vector2 FindBackPosition()
        {
            //Vector2 _towerSize = _towerTransform.GetRect();

            float xOffset;
            float yOffset;
            //float translateFactor = (_yOffset - _towerSize.y/2) / (_xOffset - _towerSize.x/2);
            if (_cardAngle != 0)
            {
                _translateFactor = Mathf.Tan((90 - _cardAngle) * Mathf.PI / 180);
                xOffset = Mathf.Sqrt(_backTranslate * _backTranslate / (_translateFactor * _translateFactor + 1));
                yOffset = xOffset * _translateFactor * (-1);
            }
            else
            {
                //translateFactor = 0;
                xOffset = 0;
                yOffset = _backTranslate * -1;
            }

            return new Vector2(CardPosition.x + xOffset, CardPosition.y + yOffset);
        }

        private Vector2 FindFirstPosition()
        {
            _xPeekToFirstCardPositionOffset = _towerSize.x / 2 + CardSize.x / 2 + CardSize.x * _offsetXFactor;
            _yPeekToFirstCardPositionOffset = _towerSize.y / 2 + CardSize.y / 2 + CardSize.y * _offsetYFactor;
            //_xOffset = towerSize.y * _offsetXFactor + cardSize.x;
            //_yOffset = towerSize.y * _offsetYFactor + cardSize.y;
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

        private Vector3 FindFirstRotation()
        {
            Vector2 currentRotation = _cardTransform.GetRotationVector();

            //Debug.Log($"_xOffset={_xOffset}, _yOffset={_yOffset},cardSize={cardSize}, towerSize={towerSize}");


            //if (_xOffset - CardSize.x/2 > _towerSize.x/2 && _yOffset - CardSize.x/2 > _towerSize.y/2)
            if (_xOffset > _towerSize.x/2 && _yOffset > _towerSize.y/2)
            {
                _cardAngle = Mathf.Atan((_xOffset - _towerSize.x/2) / (_yOffset - _towerSize.y/2)) * 180 / Mathf.PI;
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

            //Debug.Log(_cardAngle);

            Vector3 newRotation = new Vector3(currentRotation.x, currentRotation.y, _cardAngle);

            return newRotation;
        }
    }
}