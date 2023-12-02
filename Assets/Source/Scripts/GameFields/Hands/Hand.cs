using System.Collections.Generic;
using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Hands
{
    public abstract class Hand : MonoBehaviour, ICardDragListener
    {
        protected const float StartRotation = 0f;

        [SerializeField, Min(0f)] protected float OffsetX = 162.5f;
        [SerializeField] protected float HandLength = 1175f;
        [SerializeField] protected float StartPositionX = 600f;
        [SerializeField] protected float StartPositionY = 90f;
        [SerializeField] protected float StartCardTranslateSpeed = 0.5f;
        [SerializeField] protected RectTransform RectTransform;

        [SerializeField] private HandSeatPool _handSeatPool;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected List<HandSeat> HandSeats;

        private HandSeat _dragCardHandSeat;
        private int _handSeatIndex;

        public void Init()
        {
            HandSeats = new List<HandSeat>();
            _handSeatIndex = -1;

            _handSeatPool.Init();
        }

        public void OnCardDrag(Card card)
        {
            if (TryFindHandSeat(out HandSeat handSeat, card))
            {
                _dragCardHandSeat = handSeat;

                _canvasGroup.blocksRaycasts = false;
                _handSeatIndex = HandSeats.IndexOf(_dragCardHandSeat);
                HandSeats.Remove(_dragCardHandSeat);
                SortHandSeats();
            }
        }

        public void OnCardDrop()
        {
            HandSeats.Insert(_handSeatIndex, _dragCardHandSeat);
            OnEndCardDrag();
            SortHandSeats();
        }

        public void AddCard(Card card)
        {
            if (_handSeatPool.TryGetHandSeat(out HandSeat handSeat))
            {
                HandSeats.Add(handSeat);
                handSeat.SetCard(card, StartCardTranslateSpeed);
            }

            SortHandSeats();
        }

        public void RemoveCard(Card card)
        {
            _handSeatPool.ReturnObjectInPool(_dragCardHandSeat.gameObject);
            OnEndCardDrag();

            SortHandSeats();
        }

        protected abstract void SortHandSeats();

        //private void SortHandSeats()
        //{
        //    if (_handSeats.Count <= 0)
        //    {
        //        return;
        //    }

        //    float offsetX;
        //    float positionX;

        //    if (_handSeats.Count * _offsetX < _handLength / 2)
        //    {
        //        offsetX = _offsetX;
        //    }
        //    else
        //    {
        //        float xFactor = _offsetX * _handSeats.Count;
        //        float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

        //        offsetX = fullOffsetX / _handSeats.Count;
        //    }

        //    for (int i = 0; i < _handSeats.Count; i++)
        //    {
        //        positionX = _startPositionX + ((_handSeats.Count - 1) / 2f - i) * offsetX * -1;
        //        Vector3 positon = new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin);
        //        Vector3 rotation = new Vector3(0f, 0f, StartRotation);
        //        _handSeats[i].SetLocalPositionValues(positon, rotation, _startCardTranslateSpeed);
        //    }
        //}

        private void OnEndCardDrag()
        {
            _canvasGroup.blocksRaycasts = true;
            _handSeatIndex = -1;
            _dragCardHandSeat = null;
        }

        private bool TryFindHandSeat(out HandSeat findedHandSeat, Card card)
        {
            foreach (HandSeat handSeat in HandSeats)
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
            AutomaticFillComponents.DefineComponent(this, ref RectTransform, ComponentLocationTypes.InThis);
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

