using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards.Views.BigCardViews.Capabilities
{
    [RequireComponent(typeof(FadableLabel))]
    public class CapabilityDescription : MonoBehaviour, IViewable<CapabilityDescriptionActivateData>, IAutomaticFillComponents
    {
        // По опыту
        private const float ExtraWidth = 205.49f;
        private const float ExtraHeight = 156.54f;

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

            //Debug.Log("CapabilityDescription message: " + message);
            LabelActivateData fadableLabelActivateData = new LabelActivateData(message);

            _fadableLabel.Show(fadableLabelActivateData);
            // После смены text-a надо поменять width, иначе preferredHeight нормально не расчитывается
            _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _text.preferredWidth + ExtraWidth);

            // Почему именно такие величины? По опыту
            //_text.ForceMeshUpdate();
            float width = _text.preferredWidth + ExtraWidth;
            float height = _text.preferredHeight + ExtraHeight;

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