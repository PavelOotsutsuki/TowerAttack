using UnityEngine;

namespace Cards
{
    public interface ICardDropPlaceImitation : ICardSeatPlace
    {
        public Vector3 GetCentralСoordinates();
    }
}