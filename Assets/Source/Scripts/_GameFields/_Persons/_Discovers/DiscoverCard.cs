using System;
using UnityEngine;
using Tools;
using Tools.Utils.FillComponents;
using System.Collections.Generic;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverCard : MonoBehaviour, IWorkable<DiscoverCardActivateData>, IAutomaticFillComponents
    {
        //[SerializeField, Min(0f)] protected float ViewDuration = 0.5f;
        [SerializeField] protected DiscoverViewLogic ViewLogic;

        protected Action ClickCallback;
        protected IDiscoverClickHandler _discoverClickHandler;

        public bool? IsActive { get; protected set; } = null;

        public virtual void Init(Action clickCallback, IDiscoverClickHandler discoverClickHandler, float scaleFactor,
            float viewDuration)
        {
            ViewLogic.Init(viewDuration, scaleFactor);
            _discoverClickHandler = discoverClickHandler;
            ClickCallback = clickCallback;

            Deactivate();
        }

        public abstract void Deactivate();
        public abstract void Activate(DiscoverCardActivateData data);
        public abstract void StartClickActions();

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DiscoverCard))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineDiscoverViewLogic()
            };

            return list;
        }

        [ContextMenu(nameof(DefineDiscoverViewLogic))]
        private ComponentAttachInfo DefineDiscoverViewLogic()
        {
            return AutomaticFillComponents.DefineComponent(this, ref ViewLogic, ComponentLocationTypes.InThis);
        }

        #endregion
    }
}