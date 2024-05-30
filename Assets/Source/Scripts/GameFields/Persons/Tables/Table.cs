using System.Collections.Generic;
using Cards;
using GameFields.Effects;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class Table : MonoBehaviour, ICardDropPlace
    {
        [SerializeField] private TableSeat[] _cardSeats;

        private IUnbindCardManager _unbindCardManager;
        private int[] _cardSeatsSortIndices;
        private PlayedCards _playedCards;

        public virtual void Init(IUnbindCardManager unbindCardManager, EffectRoot effectRoot)
        {
            _unbindCardManager = unbindCardManager;
            _playedCards = new PlayedCards();
            InitTableSeats(effectRoot);
            SetCardSeatsIndices();
        }

        public bool TrySeatCard(Card card)
        {
            if (TryFindCardSeat(out TableSeat freeCardSeat))
            {
                _unbindCardManager.UnbindDragableCard();
                CardCharacter cardCharacter = card.PlayOnTable();
                freeCardSeat.SetCardCharacter(cardCharacter);
                _playedCards.Add(cardCharacter, card);

                return true;
            }

            return false;
        }

        public void DecreaseCardCounter()
        {
            foreach (var seat in _cardSeats)
            {
                seat.DecreaseCounter();
            }
        }

        public List<Card> GetDiscardCards()
        {
            List<Card> discardCards = new List<Card>();

            foreach (TableSeat tableSeat in _cardSeats)
            {
                if (tableSeat.IsDiscarded)
                {
                    discardCards.Add(_playedCards.GetCard(tableSeat.CardCharacter));
                }
            }

            return discardCards;
        }

        public Vector3 GetCentralСoordinates()
        {
            return transform.position;
        }

        private bool TryFindCardSeat(out TableSeat cardSeat)
        {
            cardSeat = null;

            foreach (int index in _cardSeatsSortIndices)
            {
                if (_cardSeats[index].IsEmpty)
                {
                    cardSeat = _cardSeats[index];
                    return true;
                }
            }

            return false;
        }

        private void SetCardSeatsIndices()
        {
            int countSeats = _cardSeats.Length;

            _cardSeatsSortIndices = new int[countSeats];

            for (int i = 0; i < countSeats; i++)
            {
                _cardSeatsSortIndices[i] = GetSortIndex(i, countSeats);
            }
        }

        private int GetSortIndex(int inputIndex, int countSeats)
        {
            return (countSeats + 1) / 2 + (inputIndex + 1) / 2 * ((inputIndex + 1) % 2 * 2 - 1) - 1;
        }

        private void InitTableSeats(EffectRoot effectRoot)
        {
            foreach (TableSeat tableSeat in _cardSeats)
            {
                tableSeat.Init(effectRoot);
            }
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineAllCardSeats();
        }

        [ContextMenu(nameof(DefineAllCardSeats))]
        private void DefineAllCardSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref _cardSeats);
        }
    }
}
