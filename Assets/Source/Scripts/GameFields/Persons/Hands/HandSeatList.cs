using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandSeatList : MonoBehaviour
    {
        private const float StartRotation = 0;

        [SerializeField, Min(0f)] private float _offsetX = 162.5f;
        [SerializeField] private float _handLength = 1175f;
        [SerializeField] private float _startPositionX = 600f;
        [SerializeField] private float _startPositionY = 90f;
        [SerializeField] private float _startCardTranslateSpeed = 0.5f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private HandSeatPool _handSeatPool;
        [SerializeField] private bool _isFrontCardSide;

        private List<HandSeat> _handSeats;
        private HandSeat _dragCardHandSeat;
        private int _handSeatIndex;

        private float _sortDirection;

        public void Init(float sortDirection)
        {
            _sortDirection = sortDirection;

            _handSeats = new List<HandSeat>();
            _handSeatIndex = -1;

            _handSeatPool.Init();
        }

        public bool TryGetCard(out Card card)
        {
            return TryGetRandomCard(out card);
        }

        public void DragCard(Card card)
        {
            if (TryFindHandSeat(out HandSeat handSeat, card))
            {
                _dragCardHandSeat = handSeat;

                _handSeatIndex = _handSeats.IndexOf(_dragCardHandSeat);
                _handSeats.Remove(_dragCardHandSeat);
                SortHandSeats();
            }
        }

        public void EndDragCard()
        {
            _handSeats.Insert(_handSeatIndex, _dragCardHandSeat);
            SortHandSeats();

            ResetDragOptions();
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

        public void RemoveCard()
        {
            _handSeatPool.ReturnInPool(_dragCardHandSeat);

            SortHandSeats();
            ResetDragOptions();
        }

        private bool TryGetRandomCard(out Card card)
        {
            card = null;

            if (_handSeats.Count <= 0)
            {
                return false;
            }

            int randomIndex = Random.Range(0, _handSeats.Count - 1);

            card = _handSeats[randomIndex].GetCard();

            return true;
        }

        private void ResetDragOptions()
        {
            _handSeatIndex = -1;
            _dragCardHandSeat = null;
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
    }
}
