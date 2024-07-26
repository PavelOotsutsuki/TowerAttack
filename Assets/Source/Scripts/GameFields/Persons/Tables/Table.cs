using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private TableSeat[] _cardSeats;

        private TableSeat[] _sortedSeats;
        //private PlayedCards _playedCards;

        public bool HasFreeSeat => _sortedSeats.Any(seat => seat.IsEmpty);

        public void Init()
        {
            //_playedCards = new PlayedCards();

            SetCardSeatsIndices();
        }

        public void FreeSeatBySeatable(ISeatable character)
        {
            TableSeat seat = _sortedSeats.FirstOrDefault(seat => seat.CompareCharacters(character));

            if (seat != null)
                seat.ResetCharacter();
        }

        public Dictionary<CardCharacter, Card> FreeSeats(Dictionary<CardCharacter, Card> dictionary)
        {
            Dictionary<CardCharacter, Card> newDictionary = new();

            foreach(var seat in _cardSeats)
            {
                var first = dictionary.FirstOrDefault(pair => seat.CompareCharacters(pair.Key));

                if (first.Key != null)
                {
                    newDictionary.Add(first.Key, first.Value);
                    seat.ResetCharacter();
                }
                
            }

            //TableSeat seat = _sortedSeats.FirstOrDefault(seat => seat.CompareCharacters(character));

            //if (seat != null)
            //    seat.ResetCharacter();

            return newDictionary;
        }

        public void SeatCharacter(ISeatable character)
        {
            //CardCharacter character = card.Play();
            TableSeat freeCardSeat = GetFreeSeat();
            //card.Activate(freeCardSeat.transform);
            freeCardSeat.SetCardCharacter(character);
            //_playedCards.Add(character, card);
        }

        private TableSeat GetFreeSeat() => _sortedSeats.First(seat => seat.IsEmpty);

        private void SetCardSeatsIndices()
        {
            int countSeats = _cardSeats.Length;

            _sortedSeats = new TableSeat[countSeats];

            for (int i = 0; i < countSeats; i++)
            {
                _sortedSeats[i] = _cardSeats[GetSortIndex(i, countSeats)];
            }
        }

        private static int GetSortIndex(int inputIndex, int countSeats)
        {
            return (countSeats + 1) / 2 + (inputIndex + 1) / 2 * ((inputIndex + 1) % 2 * 2 - 1) - 1;
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
