using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(Label))]
    [RequireComponent(typeof(NascentPanel))]
    public class NascentLabel : MonoBehaviour, IViewable<CancellationTokenData>, IShowable<LabelActivateDataAsync>, IAutomaticFillComponents
    {
        [SerializeField] private Label _label;
        [SerializeField] private NascentPanel _nascentPanel;

        public bool IsComplete => _nascentPanel.IsComplete;
        public bool? IsShown => _nascentPanel.IsActive;

        public void Init()
        {
            _label.Init();
            _nascentPanel.Init();
        }

        public void Show(LabelActivateDataAsync data)
        {
            _label.SetText(data.Message);
            _nascentPanel.Activate(data);
        }

        public void Show(CancellationTokenData tokenData)
        {
            _nascentPanel.Activate(tokenData);
        }

        public void Hide()
        {
            _nascentPanel.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(NascentLabel))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLabel(),
                DefineNascentPanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineLabel))]
        private ComponentAttachInfo DefineLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineNascentPanel))]
        private ComponentAttachInfo DefineNascentPanel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _nascentPanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}