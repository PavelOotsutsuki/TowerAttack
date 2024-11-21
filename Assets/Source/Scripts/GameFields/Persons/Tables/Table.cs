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

        public bool HasFreeSeat => _sortedSeats.Any(seat => seat.IsEmpty);

        public void Init()
        {
            SetCardSeatsIndices();
        }

        public List<Card> FreeSeats(IEnumerable<Card> containers)
        {
            List<Card> newContainers = new();

            foreach(var seat in _cardSeats)
            {
                Card first = containers
                    .FirstOrDefault(card => seat.CompareSeatable(card.CardCharacter));

                if (first != null)
                {
                    newContainers.Add(first);
                    seat.ResetCharacter();
                }
            }

            return newContainers;
        }

        public void SeatCharacter(ISeatable character)
        {
            TableSeat freeCardSeat = GetFreeSeat();
            freeCardSeat.SetCardCharacter(character);
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
