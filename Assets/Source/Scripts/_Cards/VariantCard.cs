using System.Collections.Generic;
using Tools;
using Tools.Settings;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class VariantCard : MonoBehaviour, IDiscoverable, IAutomaticFillComponents
    {
        [SerializeField] private VariantCardConfig _config;
        [SerializeField] private RectTransform _rectTransform;

        private CardViewData _viewData;

        public ReadOnlyRectTransform ReadOnlyRectTransform { get; private set; }
        public CardViewData ViewData => _viewData;
        public CardEffectConfig EffectConfig => _config.Effect;

        public void Init()
        {
            ReadOnlyRectTransform = new ReadOnlyRectTransform(_rectTransform);
            ReadOnlyRectTransform.SetSize(GameSettings.CardSize);

            _viewData = new CardViewData(_config.CardViewConfig);
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