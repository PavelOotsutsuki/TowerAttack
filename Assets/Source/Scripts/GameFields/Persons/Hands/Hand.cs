using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Seats;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public abstract class Hand : MonoBehaviour, IUnbindCardManager, ICardDragListener, IBlockable
    {
        private const float StartRotation = 0;

        [SerializeField, Range(-1, 1)] private float _sortDirection;

        [SerializeField, Min(0f)] private float _offsetX = 162.5f;
        [SerializeField] private float _handLength = 1175f;
        [SerializeField] private float _startPositionX = 600f;
        [SerializeField] private float _startPositionY = 90f;
        [SerializeField] private float _startCardTranslateSpeed = 0.5f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private SideType _sideType;
        [SerializeField] private bool _isActiveInteraction;

        private List<Seat> _handSeats;
        private Seat _dragCardHandSeat;
        private int _handSeatIndex;
        private SeatPool _handSeatPool;

        public void Init(SeatPool seatPool)
        {
            _handSeats = new List<Seat>();
            _handSeatIndex = -1;

            _handSeatPool = seatPool;
        }

        public void OnCardDrag(Card card)
        {
            DragCard(card);
        }

        public void OnCardDrop()
        {
            UnblockCards();
            EndDragCard(false);
        }

        public void OnCardPlay()
        {
            UnblockCards();
        }

        public void OnCardReturnInHand()
        {
            SetCardsInteraction(_handSeats);
        }

        public void UnbindDragableCard()
        {
            _handSeatPool.ReturnInPool(_dragCardHandSeat);

            SortHandSeats();
            ResetDragOptions();
        }

        public virtual void AddCard(Card card)
        {
            Seat handSeat = _handSeatPool.GetHandSeat();
            handSeat.transform.SetParent(_rectTransform);
            handSeat.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _handSeats.Add(handSeat);
            handSeat.SetCard(card, _sideType, _startCardTranslateSpeed);
            card.SetActiveInteraction(_isActiveInteraction);

            SortHandSeats();
        }

        public bool TryGetCard(out Card card)
        {
            return TryGetRandomCard(out card);
        }

        public void ForciblyBlock()
        {
            EndDragCard(true);
            BlockCards();
        }

        public void Unblock()
        {
            UnblockCards();
        }

        private void UnblockCards()
        {
            _isActiveInteraction = true;

            SetCardsInteraction(_handSeats);
        }

        private void EndDragCard(bool isForced)
        {
            if (_handSeatIndex != -1)
            {
                if (isForced)
                {
                    Card dragCard = _dragCardHandSeat.Card;

                    dragCard.EndDrag();
                    dragCard.SetActiveInteraction(false);
                }

                _handSeats.Insert(_handSeatIndex, _dragCardHandSeat);

                SortHandSeats();
                ResetDragOptions();
            }
        }

        private void DragCard(Card card)
        {
            if (TryFindHandSeat(out Seat handSeat, card))
            {
                _dragCardHandSeat = handSeat;

                _handSeatIndex = _handSeats.IndexOf(_dragCardHandSeat);
                _handSeats.Remove(_dragCardHandSeat);

                BlockCards();
                SortHandSeats();
            }
        }

        private void BlockCards()
        {
            _isActiveInteraction = false;

            SetCardsInteraction(_handSeats);
        }

        private void BlockCards(ICollection<Card> exceptions)
        {
            _isActiveInteraction = false;

            IEnumerable<Seat> seats = _handSeats.Where(seat => exceptions.Contains(seat.Card) == false);

            SetCardsInteraction(seats);
        }

        private void SetCardsInteraction(IEnumerable<Seat> seats)
        {
            foreach (Seat seat in seats)
            {
                Card card = seat.Card;

                card.SetActiveInteraction(_isActiveInteraction);
            }
        }

        private void ResetDragOptions()
        {
            _handSeatIndex = -1;
            _dragCardHandSeat = null;
        }

        #region Comments

        //private void BlockActiveSeatsCards(List<Card> exceptions)
        //{
        //    _isActiveInteraction = false;

        //    foreach (Seat seat in _handSeats)
        //    {
        //        bool isException = false;
        //        Card card = seat.GetCard();

        //        foreach (Card exceptionCard in exceptions)
        //        {
        //            if (card == exceptionCard)
        //            {
        //                isException = true;
        //            }
        //        }

        //        if (isException == false)
        //        {
        //            card.SetActiveInteraction(false);
        //        }
        //    }
        //}

        //private void BlockActiveSeatsCards()
        //{
        //    _isActiveInteraction = false;

        //    SetCardsInteraction();
        //}

        //private void BlockCards()
        //{
        //    if (_handSeatIndex != -1)
        //    {
        //        Card dragCard = _dragCardHandSeat.GetCard();

        //        dragCard.EndDrag();
        //        dragCard.SetActiveInteraction(false);
        //    }

        //    BlockActiveSeatsCards();
        //}

        #endregion

        #region Right

        private bool TryGetRandomCard(out Card card)
        {
            card = null;

            if (_handSeats.Count <= 0)
            {
                return false;
            }

            int randomIndex = Random.Range(0, _handSeats.Count);

            card = _handSeats[randomIndex].Card;

            return true;
        }

        private bool TryFindHandSeat(out Seat findedHandSeat, Card card)
        {
            foreach (Seat handSeat in _handSeats)
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

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        #endregion 
    }
}
