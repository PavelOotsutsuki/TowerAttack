using System.Collections.Generic;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    [RequireComponent(typeof(FadableLabel))]
    public class CapabilityDescription : MonoBehaviour, IViewable<CapabilityDescriptionActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private TMP_Text _text;
        [SerializeField] private RectTransform _rectTransform;
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

            // Почему именно такие величины? По опыту
            float width = _text.preferredWidth + 205.49f;
            float height = _text.preferredHeight + 156.54f;

            _rectTransform.sizeDelta = new Vector2(width, height);

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
                DefineText(),
                DefineRectTransform(),
                DefineFadableLabel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineText))]
        private ComponentAttachInfo DefineText()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _text, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFadableLabel))]
        private ComponentAttachInfo DefineFadableLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadableLabel, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}