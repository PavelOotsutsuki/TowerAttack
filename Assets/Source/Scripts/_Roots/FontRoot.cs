using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;
using TMPro;

namespace Roots
{
    internal class FontRoot : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private TMP_Text[] _allTextMechProTexts;
        [SerializeField] private TMP_FontAsset _defaultFont;

        public void Init()
        {
            foreach (TMP_Text TMP_Text in _allTextMechProTexts)
            {
                TMP_Text.font = _defaultFont;
                //TMP_Text.text = "АБВГДЕЁЖЗИЙКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзийклмнопрстуфхцчшщьыъэюя1234567890,.!? -:;";
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FontRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAllTMP_Texts()
            };

            return list;
        }

        [ContextMenu(nameof(DefineAllTMP_Texts))]
        private ComponentAttachInfo DefineAllTMP_Texts()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _allTextMechProTexts, true);
        }
        #endregion
    }
}