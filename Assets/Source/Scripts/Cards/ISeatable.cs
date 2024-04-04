using UnityEngine;

namespace Cards
{
    public interface ISeatable
    {
        public void BindSeat(Transform transform, bool isFrontSide, float duration = 0f);
    }
}
