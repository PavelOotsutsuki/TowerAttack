using System.Linq;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class Table : MonoBehaviour
    {
        [SerializeField] private TableSeat[] _cardSeats;

        private int[] _cardSeatsSortIndices;

        public bool HasFreeSeat => _cardSeatsSortIndices.Any(index => _cardSeats[index].IsEmpty);

        public void Init()
        {
            SetCardSeatsIndices();
        }

        public void FreeSeatByCharacter(GameObject character)
        {
            var seat = _cardSeats.FirstOrDefault(seat => seat.CompareCharacters(character));

            if (seat != null)
                seat.ResetCharacter();
        }

        public void SeatCharacter(CardCharacter character)
        {
            if (TryFindCardSeat(out TableSeat freeCardSeat))
            {
                character.Activate(freeCardSeat.transform);
                freeCardSeat.SetCardCharacter(character.gameObject);
            }
            else
            {
                throw new System.NullReferenceException("Не найден TableSeat");
            }
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

            return cardSeat != null;
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
