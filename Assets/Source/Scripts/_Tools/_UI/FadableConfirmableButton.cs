using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadablePanel))]
    public class FadableConfirmableButton : ConfirmableButton, ICompletable, IWorkable
    {
        [SerializeField] private FadablePanel _fadablePanel;

        public bool IsComplete => _fadablePanel.IsComplete;

        public override void Init()
        {
            base.Init();

            _fadablePanel.Init();
        }

        public virtual void Activate()
        {
            _fadablePanel.Show();
        }

        public virtual void Deactivate()
        {
            _fadablePanel.Hide();
        }

        #region AutomaticFillComponents
        protected override void DefineAllComponents()
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