using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;
using TMPro;
using Tools;
using System.Linq;

namespace Roots
{
    public class FontRoot : MonoBehaviour, IFontSetter, IAutomaticFillComponents
    {
        [SerializeField] private TMP_Text[] _allTextMechProTexts;
        [SerializeField] private TMP_FontAsset _defaultFont;
        [SerializeField] private TMP_Text[] _exceptions;

        internal void Init()
        {
            SetFont(_allTextMechProTexts);
        }

        public void SetFont(IEnumerable<TMP_Text> texts)
        {
            if (texts != null)
                if (texts.Count() > 0)
                    foreach (TMP_Text TMP_Text in texts)
                    {
                        if (_exceptions.Contains(TMP_Text) == false)
                            TMP_Text.font = _defaultFont;
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