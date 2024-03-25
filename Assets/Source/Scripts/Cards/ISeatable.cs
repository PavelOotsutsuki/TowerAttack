using UnityEngine;

namespace Cards
{
    public interface ISeatable
    {
        public void BindSeat(Transform seat, bool isFrontSide, float duration = 0f);
    }
}
