using TMPro;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools
{
    [RequireComponent(typeof(FadablePanel))]
    public class FadableLabel : MonoBehaviour, ICompletable
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private FadablePanel _fadablePanel;

        [SerializeField] private FadableLableData _data;

        public bool IsComplete => _fadablePanel.IsComplete;

        public void Init()
        {
            _text.text = _data.StartText;

            _fadablePanel.Init();
        }

        public void Show(string description)
        {
            Show();

            _text.text = description;
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
        [ContextMenu(nameof(DefineAllComponentsFadableLabel))]
        private void DefineAllComponentsFadableLabel()
        {
            DefineText();
            DefineFadablePanel();
        }

        [ContextMenu(nameof(DefineText))]
        private void DefineText()
        {
            AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private void DefineFadablePanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}