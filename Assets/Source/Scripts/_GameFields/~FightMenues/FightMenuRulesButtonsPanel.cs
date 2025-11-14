using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Cards;
using Cards.Views;
using Cards.Views.BigCardViews.Capabilities;
using Cysharp.Threading.Tasks;
using GameFields.FightMenues;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.FightMenues
{
    public class FightMenuRulesButtonsPanel : FightMenuButtonsPanel, IAutomaticFillComponents
    {
        [SerializeField] private GoBackOnMainPanelButton _goBackOnMainPanelButton;
        [SerializeField] private TMP_Text _label;
        [SerializeField] private ScrollRect _scrollRect;

        public override bool? IsActive { get; protected set; } = null;

        public void Init(Action onClickGoBackOnMainPanelButton, CardCapabilityDescription cardCapabilityDescription)
        {
            _goBackOnMainPanelButton.Init(onClickGoBackOnMainPanelButton);

            _label.text = cardCapabilityDescription.GetAllCapabilitiesDescription((CardCapability)(-1));
        }

        public override void Activate()
        {
            if (IsActive == true)
                return;

            base.Activate();

            float height = _label.preferredHeight;

            RectTransform content = _scrollRect.content;
            RectTransform scrollRect = _scrollRect.transform as RectTransform; // Хуйня, но вроде и серилизовать её тоже хуйня

            content.sizeDelta = new Vector2(content.offsetMax.x * (-1f), height);
            content.anchoredPosition = new Vector2(content.anchoredPosition.x, scrollRect.rect.height);
            _scrollRect.verticalScrollbar.Select();
        }

        public override void OnDownArrow()
        { }

        public override void OnEnterPress()
        { }

        public override void OnUpArrow()
        { }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FightMenuRulesButtonsPanel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineGoBackOnMainPanelButton(),
                DefineTMP_Text(),
                DefineScrollRect()
            };

            return list;
        }

        [ContextMenu(nameof(DefineGoBackOnMainPanelButton))]
        private ComponentAttachInfo DefineGoBackOnMainPanelButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _goBackOnMainPanelButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTMP_Text))]
        private ComponentAttachInfo DefineTMP_Text()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _label, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineScrollRect))]
        private ComponentAttachInfo DefineScrollRect()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _scrollRect, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}