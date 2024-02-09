using UnityEngine;

namespace Cards
{
    public interface ICardDropPlaceImitation : ICardGetPlace
    {
        public Vector3 GetCentralСoordinates();
    }
}
