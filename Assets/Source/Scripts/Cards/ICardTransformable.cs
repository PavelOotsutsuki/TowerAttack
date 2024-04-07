using UnityEngine;

namespace Cards
{
    public interface ICardTransformable
    {
        public RectTransform Transform { get; }
        public Vector3 DefaultScaleVector { get; }

        public void SetSide(SideType sideType);
    }
}
