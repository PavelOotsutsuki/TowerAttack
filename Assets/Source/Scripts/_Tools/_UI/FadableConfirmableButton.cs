using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadablePanel))]
    public abstract class FadableConfirmableButton : ConfirmableButton, ICompletable, IWorkable
    {
        [SerializeField] private FadablePanel _fadablePanel;

        public bool IsComplete => _fadablePanel.IsComplete;

        public override void Init()
        {
            base.Init();

            _fadablePanel.Init();
        }

        public override void Activate()
        {
            base.Activate();

            _fadablePanel.Show();
        }

        public override void Deactivate()
        {
            base.Deactivate();

            _fadablePanel.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FadableConfirmableButton))]
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