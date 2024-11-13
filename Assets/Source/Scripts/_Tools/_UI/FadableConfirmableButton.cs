using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadablePanel))]
    public class FadableConfirmableButton : ConfirmableButton, ICompletable, IWorkable
    {
        [SerializeField] private FadablePanel _fadablePanel;

        public bool IsComplete => _fadablePanel.IsComplete;
        public bool? IsActive => _fadablePanel.IsShown;

        public override void Init()
        {
            base.Init();

            _fadablePanel.Init();
        }

        public override void Activate()
        {
            _fadablePanel.Show();

            base.Activate();
        }

        public virtual void Deactivate()
        {
            _fadablePanel.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FadableConfirmableButton))]
        public override void DefineAllComponents()
        {
            DefineFadablePanel();

            base.DefineAllComponents();
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private void DefineFadablePanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}