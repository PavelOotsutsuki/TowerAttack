using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerActivator : MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private CanvasGroup _canvasGroup;

        public bool? IsActive { get; private set; } = null;

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _canvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _canvasGroup.blocksRaycasts = false;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerActivator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}