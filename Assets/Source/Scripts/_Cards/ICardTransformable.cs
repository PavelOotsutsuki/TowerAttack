using Tools;
using Tools.Utils.Movements;
using UnityEngine;

namespace Cards
{
    public interface ICardTransformable
    {
        public ReadOnlyRectTransform ReadOnlyRectTransform { get; }
        public Movement CardMovement {get;}
        public Vector3 DefaultScaleVector { get; }

        public void SetSide(SideType sideType);
        public void SetActiveInteraction(bool isActive);
    }
}