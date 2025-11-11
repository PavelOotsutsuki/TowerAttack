using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    [RequireComponent(typeof(FadableLabel))]
    public class CapabilityDescription : MonoBehaviour, IViewable<CapabilityDescriptionActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private FadableLabel _fadableLabel;

        private CardCapabilityDescription _cardCapabilityDescription;

        public bool? IsShown { get; private set; } = false;

        public void Init(CardCapabilityDescription cardCapabilityDescription)
        {
            _cardCapabilityDescription = cardCapabilityDescription;

            _fadableLabel.Init();
        }

        public void Show(CapabilityDescriptionActivateData data)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            string message = _cardCapabilityDescription.GetAllCapabilitiesToStringValue(data.CardCapability);

            if (message == "")
            {
                IsShown = false;
                return;
            }

            LabelActivateData fadableLabelActivateData = new LabelActivateData(message);

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
        [ContextMenu(nameof(DefineAllComponents) + nameof(CapabilityDescription))]
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