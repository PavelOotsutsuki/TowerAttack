using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    [RequireComponent(typeof(FadableLabel))]
    [RequireComponent(typeof(NascentPanel))]
    public class FadableNascentLabel : MonoBehaviour
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
        protected virtual void DefineAllComponents()
        {
            DefineFadableLabel();
            DefineNascentPanel();
        }

        [ContextMenu(nameof(DefineFadableLabel))]
        private void DefineFadableLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineNascentPanel))]
        private void DefineNascentPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _nascentPanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}