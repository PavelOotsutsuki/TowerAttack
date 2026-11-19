using System;
using System.Collections.Generic;
using System.Threading;
using Cards.Views;
using Cards.Views.BigCardViews.BigCards;
using Cards.Views.BigCardViews.CardDescriptions;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Cards.Insides
{
    internal class CardCharacter : MonoBehaviour, ICardState
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private CardFeatureHelper _cardFeatureHelper;

        public bool? IsShown { get; private set; } = null;

        //public void Init(Sprite icon, CardDescription cardDescription, IFeatureWatcher featureWatcher, CancellationToken cardToken)
        //{
        //    _iconImage.sprite = icon;
        //    transform.localPosition = Vector2.zero;
        //    _cardFeatureHelper.Init(cardDescription, () => featureWatcher.Feature, cardToken);

        //    IsShown = true;
        //    Hide();
        //}

        private DateTime? _lastActivate = null;

        public void Init(Sprite icon, CardViewService cardViewService, Vector2 cardSize, ReadOnlyRectTransform RORTransform, CardViewData cardViewData,
            CancellationToken cardToken)
        {
            _iconImage.sprite = icon;
            transform.localPosition = Vector2.zero;
            _cardFeatureHelper.Init(cardViewService, () => new BigCardShowData(cardSize, RORTransform, cardViewData, BigCardPositionType.RightUp), cardToken, IsDelayTimeAfterActivateIsOver);

            IsShown = true;
            Hide();
        }

        //private bool IsDelayTimeAfterActivateIsOver => _lastActivate == null ? true : (DateTime.Now - _lastActivate.Value).Milliseconds > 100;

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;
            _lastActivate = DateTime.Now;

            gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _cardFeatureHelper.OnPointerExit(new PointerEventData(EventSystem.current));
            gameObject.SetActive(false);
        }

        private bool IsDelayTimeAfterActivateIsOver()
        {
            DateTime now = DateTime.Now;

            Debug.Log("(DateTime.Now - _lastActivate.Value).Milliseconds: " + (now - _lastActivate.Value).TotalMilliseconds);

            return _lastActivate == null ? true : (now - _lastActivate.Value).TotalMilliseconds > 200;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardCharacter))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineCardFeatureHelper()
            };

            return list;
        }

        [ContextMenu(nameof(DefineCardFeatureHelper))]
        private ComponentAttachInfo DefineCardFeatureHelper()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardFeatureHelper, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}