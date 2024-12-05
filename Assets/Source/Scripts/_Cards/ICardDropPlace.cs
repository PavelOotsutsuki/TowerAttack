using Tools;
using UnityEngine;

namespace Cards
{
    public interface ICardDropPlace
    {
        public bool HasFreeSeat { get; }
        public ReadOnlyRectTransform ReadOnlyRectTransform { get; }

        public void SeatCard(Card card);
    }
}