using UnityEngine;

namespace Cards
{
    public interface ICardDropPlace
    {
        public bool HasFreeSeat { get; }

        public void SeatCard(Card card);
        public Vector3 GetPosition();
    }
}