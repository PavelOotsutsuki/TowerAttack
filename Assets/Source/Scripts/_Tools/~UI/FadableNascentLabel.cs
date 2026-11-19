using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadableLabel))]
    [RequireComponent(typeof(NascentPanel))]
    public class FadableNascentLabel : MonoBehaviour, IAutomaticFillComponents, IShowable<LabelActivateDataAsync>, IViewable<CancellationTokenData, CancellationTokenData>
    {
        [SerializeField] private FadableLabel _fadableLabel;
        [SerializeField] private NascentPanel _nascentPanel;

        public bool IsComplete => _fadableLabel.IsComplete && _nascentPanel.IsComplete;

        public bool? IsShown { get; private set; } = null;

        public void Init()
        {
            _fadableLabel.Init();
            _nascentPanel.Init();
        }

        public void Show(LabelActivateDataAsync data)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            _fadableLabel.Show(data);
            _nascentPanel.Activate(data);
        }

        public void Show(CancellationTokenData tokenData)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            _fadableLabel.Show(tokenData);
            _nascentPanel.Activate(tokenData);
        }

        public void Hide(CancellationTokenData tokenData)
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _fadableLabel.Hide(tokenData);
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