using TMPro;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    public class Label : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private TMP_Text _text;

        [SerializeField] private LableData _data; 

        public void Init()
        {
            _text.text = _data.StartText;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Label))]
        public void DefineAllComponents()
        {
            DefineText();
        }

        [ContextMenu(nameof(DefineText))]
        private void DefineText()
        {
            AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InThisElseChildren);
        }
        #endregion
    }
}