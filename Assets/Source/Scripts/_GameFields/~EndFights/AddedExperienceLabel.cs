using System.Collections.Generic;
using System.Threading;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.EndFights
{
    [RequireComponent(typeof(NascentLabel))]
    public class AddedExperienceLabel : MonoBehaviour, ICompletable, IShowable<AddedExperienceLabelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private NascentLabel _label;
        [SerializeField] private TMP_Text _TMPtext;

        private CancellationToken _gameFieldToken;

        public bool IsComplete => _label.IsComplete;

        public void Init(CancellationToken gameFieldToken)
        {
            _gameFieldToken = gameFieldToken;

            _label.Init();
        }

        public void Show(AddedExperienceLabelActivateData data)
        {
            _TMPtext.color = data.TextColor;

            _label.Show(new LabelActivateDataAsync(data.NascentLabelActivateData, _gameFieldToken));
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(EndFightLabel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineNascentLabel(),
                DefineTMP_Text()
            };

            return list;
        }

        [ContextMenu(nameof(DefineNascentLabel))]
        private ComponentAttachInfo DefineNascentLabel()
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