using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public abstract class Hand : MonoBehaviour
    {
        private const float StartRotation = 0;

        [SerializeField] protected CanvasGroup CanvasGroup;

        [SerializeField, Min(0f)] private float _offsetX = 162.5f;
        [SerializeField] private float _handLength = 1175f;
        [SerializeField] private float _startPositionX = 600f;
        [SerializeField] private float _startPositionY = 90f;
        [SerializeField] private float _startCardTranslateSpeed = 0.5f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private HandSeatPool _handSeatPool;
        [SerializeField] private bool _isFrontCardSide;

        protected List<HandSeat> HandSeats;
        protected HandSeat DragCardHandSeat;
        protected int HandSeatIndex;

        public virtual void Init()
        {
            HandSeats = new List<HandSeat>();
            HandSeatIndex = -1;

            _handSeatPool.Init();
        }

        public virtual void RemoveCard(Card card) // Тут что-то явно надо поменять
        {
            _handSeatPool.ReturnInPool(DragCardHandSeat);

            SortHandSeats();
        }

        public void AddCard(Card card)
        {
            if (_handSeatPool.TryGetHandSeat(out HandSeat handSeat))
            {
                HandSeats.Add(handSeat);
                handSeat.SetCard(card, _isFrontCardSide, _startCardTranslateSpeed);
            }

            SortHandSeats();
        }

        protected void SortHandSeats()
        {
            if (HandSeats.Count <= 0)
            {
                return;
            }

            float offsetX;
            float positionX;

            if (HandSeats.Count * _offsetX < _handLength / 2)
            {
                offsetX = _offsetX;
            }
            else
            {
                float xFactor = _offsetX * HandSeats.Count;
                float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

                offsetX = fullOffsetX / HandSeats.Count;
            }

            offsetX *= -1 * GetSortDirection();

            for (int i = 0; i < HandSeats.Count; i++)
            {
                positionX = _startPositionX + ((HandSeats.Count - 1) / 2f - i) * offsetX;
                Vector3 positon = new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin);
                Vector3 rotation = new Vector3(0f, 0f, StartRotation);
                HandSeats[i].SetLocalPositionValues(positon, rotation, _startCardTranslateSpeed);
            }
        }

        protected bool TryFindHandSeat(out HandSeat findedHandSeat, Card card)
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

        protected abstract float GetSortDirection();

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
            AutomaticFillComponents.DefineComponent(this, ref CanvasGroup, ComponentLocationTypes.InThis);
        }
    }
}
