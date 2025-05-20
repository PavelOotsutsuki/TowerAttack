using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverViewLogic: MonoBehaviour, ICompletable, IShowable<DiscoverViewLogicData>, IAutomaticFillComponents
    {
        [SerializeField] protected RectTransform RectTransform;
        [SerializeField, Min(1f)] protected float ScaleFactor = 2f;

        protected float Duration;
        protected Movement Movement;

        public bool IsComplete { get; protected set; }

        public virtual void Init(float duration)
        {
            IsComplete = false;
            Duration = duration;
            RectTransform.rotation = Quaternion.identity;
            RectTransform.localPosition = Vector3.zero;
            Movement = new Movement(RectTransform);
        }

        public abstract void Show(DiscoverViewLogicData discoverViewLogicData);

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DiscoverViewLogic))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref RectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}