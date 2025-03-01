using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.InformationLabels
{
    public class InformationLableRoot : MonoBehaviour, IWorkable<LabelActivateData>, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private InformationLabel _informationLabel;
        [SerializeField] private InformationLabelPanel _panel;

        private bool _isComplete;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete => _isComplete && _informationLabel.IsComplete && _panel.IsComplete;

        public void Init()
        {
            _informationLabel.Init();
            _panel.Init();
        }

        public void Activate(LabelActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _isComplete = false;

            _informationLabel.Show(data);
            _panel.Show();

            _isComplete = true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _isComplete = false;

            _informationLabel.Hide();
            _panel.Hide();

            _isComplete = true;
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