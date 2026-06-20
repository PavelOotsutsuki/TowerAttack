using System.Collections.Generic;
using Cards.Effects;
using Tools;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards.Views
{
    public class VariantCard : MonoBehaviour, IDiscoverable, IAutomaticFillComponents
    {
        [SerializeField] private VariantCardConfig _config;
        [SerializeField] private RectTransform _rectTransform;

        private CardViewData _viewData;

        public ReadOnlyRectTransform RORTransform { get; private set; }
        public CardViewData ViewData => _viewData;
        public CardEffectConfig EffectConfig => _config.Effect;

        public void Init()
        {
            RORTransform = new ReadOnlyRectTransform(_rectTransform);
            RORTransform.SetSize(GameSettings.CardSize);

            _viewData = new CardViewData(_config.CardViewConfig, _config.CardCapability);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(VariantCard))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}