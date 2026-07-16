using System.Collections.Generic;
using TMPro;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Tools.UI
{
    public class Label : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private TMP_Text _text;

        [SerializeField] private LableData _data;

        //public int TextLength => _text.text.Length;

        public void Init()
        {
            _text.text = _data.StartText;
        }

        public void SetText(string text)
        {
            _text.text = text;
        }

        public void SetColor(Color color)
        {
            _text.color = color;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Label))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineText()
            };

            return list;
        }

        [ContextMenu(nameof(DefineText))]
        private ComponentAttachInfo DefineText()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InThisElseChildren);
        }
        #endregion
    }
}