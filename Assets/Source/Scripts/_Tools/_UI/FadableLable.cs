using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadablePanel))]
    [RequireComponent(typeof(Label))]
    public class FadableLabel : MonoBehaviour, ICompletable, IViewable, IShowable<FadableLabelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private Label _label;
        [SerializeField] private FadablePanel _fadablePanel;

        public bool IsComplete => _fadablePanel.IsComplete;
        public bool? IsShown => _fadablePanel.IsShown;

        public virtual void Init()
        {
            _label.Init();
            _fadablePanel.Init();
        }

        public void Show(FadableLabelActivateData data)
        {
            Show();

            _label.SetText(data.Message);
        }

        public void Show()
        {
            _fadablePanel.Show();
        }

        public void Hide()
        {
            _fadablePanel.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FadableLabel))]
        public virtual void DefineAllComponents()
        {
            DefineLabel();
            DefineFadablePanel();
        }

        [ContextMenu(nameof(DefineLabel))]
        private void DefineLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private void DefineFadablePanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}