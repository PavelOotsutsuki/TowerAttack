using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadablePanel))]
    public class FadableSelectableButton : SelectableButton, ICompletable, IWorkable<CancellationTokenData, CancellationTokenData>
    {
        [SerializeField] private FadablePanel _fadablePanel;

        public bool IsComplete => _fadablePanel.IsComplete;

        public override void Init()
        {
            base.Init();

            _fadablePanel.Init();
        }

        public void Activate(CancellationTokenData tokenData)
        {
            base.BaseActivate();

            _fadablePanel.Show(tokenData);
        }

        public void Deactivate(CancellationTokenData tokenData)
        {
            base.BaseDeactivate();

            _fadablePanel.Hide(tokenData);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FadableSelectableButton))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadablePanel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}