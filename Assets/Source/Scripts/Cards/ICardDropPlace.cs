using UnityEngine;

namespace Cards
{
    public interface ICardDropPlace
    {
        public bool TrySeatCard(Card card);
        public Vector3 GetCentralСoordinates();
    }
}
