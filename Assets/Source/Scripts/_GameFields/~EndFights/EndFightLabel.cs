using System.Collections.Generic;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.EndFights
{
    [RequireComponent(typeof(NascentLabel))]
    public class EndFightLabel : MonoBehaviour, ICompletable, IShowable<EndFightLabelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private NascentLabel _label;
        [SerializeField] private TMP_Text _TMPtext;

        public bool IsComplete => _label.IsComplete;

        public void Init()
        {
            _label.Init();
        }

        public void Show(EndFightLabelActivateData data)
        {
            _TMPtext.color = data.TextColor;

            _label.Show(data.NascentLabelActivateData);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(EndFightLabel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineEndFightPanel(),
                DefineTMP_Text()
            };

            return list;
        }

        [ContextMenu(nameof(DefineEndFightPanel))]
        private ComponentAttachInfo DefineEndFightPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineTMP_Text))]
        private ComponentAttachInfo DefineTMP_Text()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _TMPtext, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}