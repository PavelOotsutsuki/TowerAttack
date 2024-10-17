using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverViewLogic: MonoBehaviour
    {
        [SerializeField] protected RectTransform RectTransform;
        [SerializeField, Min(1f)] protected float ScaleFactor = 2f;

        protected float Duration;

        protected Movement Movement;

        public virtual void Init(float duration)
        {
            Duration = duration;
            RectTransform.rotation = Quaternion.identity;
            RectTransform.localPosition = Vector3.zero;
            Movement = new Movement(RectTransform);
        }

        public abstract void View(float cardHeight, float cardWidth);

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref RectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}