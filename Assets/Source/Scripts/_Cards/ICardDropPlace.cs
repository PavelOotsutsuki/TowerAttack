using Tools;
using UnityEngine;

namespace Cards
{
    public interface ICardDropPlace: IReadOnlyRectTransformable
    {
        public bool HasFreeSeat { get; }

        public void SeatCard(Card card);
    }
}