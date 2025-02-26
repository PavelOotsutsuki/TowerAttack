using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.InformationLabels
{
    public class InformationLableRoot : MonoBehaviour, IWorkable<FadableLabelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private InformationLabel _informationLabel;
        [SerializeField] private InformationLabelPanel _panel;

        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            _informationLabel.Init();
            _panel.Init();
        }

        public void Activate(FadableLabelActivateData data)
        {
            _informationLabel.Show(data);
            _panel.Show();
        }

        public void Deactivate()
        {
            _informationLabel.Hide();
            _panel.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(InformationLableRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineInformationLabel(),
                DefineInformationLabelPanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineInformationLabel))]
        private ComponentAttachInfo DefineInformationLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _informationLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineInformationLabelPanel))]
        private ComponentAttachInfo DefineInformationLabelPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _panel, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}