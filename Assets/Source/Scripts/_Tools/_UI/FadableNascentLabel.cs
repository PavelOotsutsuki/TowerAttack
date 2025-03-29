using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadableLabel))]
    [RequireComponent(typeof(NascentPanel))]
    public class FadableNascentLabel : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private FadableLabel _fadableLabel;
        [SerializeField] private NascentPanel _nascentPanel;

        public bool IsComplete => _fadableLabel.IsComplete && _nascentPanel.IsComplete;

        public void Init()
        {
            _fadableLabel.Init();
            _nascentPanel.Init();
        }

        public void Show(FadableLabelActivateData data)
        {
            _fadableLabel.Show(data);
            _nascentPanel.Activate();
        }

        public void Show()
        {
            _fadableLabel.Show();
            _nascentPanel.Activate();
        }

        public void Hide()
        {
            _fadableLabel.Hide();
            _nascentPanel.Deactivate();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FadableNascentLabel))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadableLabel(),
                DefineNascentPanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadableLabel))]
        private ComponentAttachInfo DefineFadableLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineNascentPanel))]
        private ComponentAttachInfo DefineNascentPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _nascentPanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}