using System.Collections.Generic;
using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Persons.Hands
{
    public class Hand : MonoBehaviour, ICardDragListener
    {
        private const float StartRotation = 0;

        [SerializeField, Min(0f)] private float _offsetX = 162.5f;
        [SerializeField] private float _handLength = 1175f;
        [SerializeField] private float _startPositionX = 600f;
        [SerializeField] private float _startPositionY = 90f;
        [SerializeField] private float _startCardTranslateSpeed = 0.5f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private HandSeatPool _handSeatPool;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private bool _isFrontCardSide;

        private List<HandSeat> _handSeats;
        private HandSeat _dragCardHandSeat;
        private int _handSeatIndex;
        private float _sortDirection;
        private HandOwner _handOwner;

        public void Init(HandOwner handOwner)
        {
            _handOwner = handOwner;

            SetSortDirection();

            _handSeats = new List<HandSeat>();
            _handSeatIndex = -1;

            _handSeatPool.Init();
        }

        public void OnCardDrag(Card card)
        {
            if (TryFindHandSeat(out HandSeat handSeat, card))
            {
                _dragCardHandSeat = handSeat;

                _canvasGroup.blocksRaycasts = false;
                _handSeatIndex = _handSeats.IndexOf(_dragCardHandSeat);
                _handSeats.Remove(_dragCardHandSeat);
                SortHandSeats();
            }
        }

        public void OnCardDrop()
        {
            _handSeats.Insert(_handSeatIndex, _dragCardHandSeat);
            OnEndCardDrag();
            SortHandSeats();
        }

        public void AddCard(Card card)
        {
            if (_handSeatPool.TryGetHandSeat(out HandSeat handSeat))
            {
                _handSeats.Add(handSeat);
                handSeat.SetCard(card, _isFrontCardSide, _startCardTranslateSpeed);
            }

            SortHandSeats();
        }

        public void RemoveCard(Card card)
        {
            _handSeatPool.ReturnInPool(_dragCardHandSeat);
            OnEndCardDrag();

            SortHandSeats();
        }

        private void SetSortDirection()
        {
            _sortDirection = _handOwner switch
            {
                HandOwner.Player => 1,
                HandOwner.Enemy => -1,
                _ => throw new System.NotImplementedException()
            };
        }
            
        private void SortHandSeats()
        {
            if (_handSeats.Count <= 0)
            {
                return;
            }

            float offsetX;
            float positionX;

            if (_handSeats.Count * _offsetX < _handLength / 2)
            {
                offsetX = _offsetX;
            }
            else
            {
                float xFactor = _offsetX * _handSeats.Count;
                float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

                offsetX = fullOffsetX / _handSeats.Count;
            }

            offsetX *= -1 * _sortDirection;

            for (int i = 0; i < _handSeats.Count; i++)
            {
                positionX = _startPositionX + ((_handSeats.Count - 1) / 2f - i) * offsetX;
                Vector3 positon = new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin);
                Vector3 rotation = new Vector3(0f, 0f, StartRotation);
                _handSeats[i].SetLocalPositionValues(positon, rotation, _startCardTranslateSpeed);
            }
        }

        private void OnEndCardDrag()
        {
            _canvasGroup.blocksRaycasts = true;
            _handSeatIndex = -1;
            _dragCardHandSeat = null;
        }

        private bool TryFindHandSeat(out HandSeat findedHandSeat, Card card)
        {
            foreach (HandSeat handSeat in _handSeats)
            {
                if (handSeat.IsCardEqual(card))
                {
                    findedHandSeat = handSeat;
                    return true;
                }
            }

            findedHandSeat = null;
            return false;
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
            DefineHandSeatPool();
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineHandSeatPool))]
        private void DefineHandSeatPool()
        {
            AutomaticFillComponents.DefineComponent(this, ref _handSeatPool, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
    }
}

