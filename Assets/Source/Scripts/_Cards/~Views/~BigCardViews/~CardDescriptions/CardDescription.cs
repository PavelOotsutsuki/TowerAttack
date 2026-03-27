using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards.Views.BigCardViews.CardDescriptions
{
    [RequireComponent(typeof(FadableLabel))]
    internal class CardDescription : MonoBehaviour, IViewable<CardDescriptionActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private FadableLabel _fadableLabel;

        public bool? IsShown { get; private set; } = false;

        public void Init()
        {
            gameObject.SetActive(true);

            _fadableLabel.Init();
        }

        public void Show(CardDescriptionActivateData data)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            LabelActivateData fadableLabelActivateData = new LabelActivateData(data.Description);

            _fadableLabel.Show(fadableLabelActivateData);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _fadableLabel.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardDescription))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadableLabel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadableLabel))]
        private ComponentAttachInfo DefineFadableLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}