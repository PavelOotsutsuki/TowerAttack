using System.Collections.Generic;
using DG.Tweening;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    public class NascentPanel : MonoBehaviour, ICompletable, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private NascentData _data;

        public bool IsComplete { get; private set; }
        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            IsComplete = true;

            Deactivate();
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;
            IsComplete = false;

            _transform.DOScale(_data.EndScale, _data.Duration).OnComplete(() => IsComplete = true);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            _transform.localScale = _data.StartScale;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(NascentPanel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}