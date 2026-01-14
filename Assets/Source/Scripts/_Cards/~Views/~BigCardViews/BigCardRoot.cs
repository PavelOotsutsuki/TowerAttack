using System.Collections.Generic;
using Cards.Views.BigCardViews.BigCards;
using Cards.Views.BigCardViews.Capabilities;
using Cards.Views.BigCardViews.CardDescriptions;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards.Views.BigCardViews
{
    public class BigCardRoot : MonoBehaviour, IWorkable<BigCardRootActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private CapabilityDescription _capabilityDescription;
        [SerializeField] private CardDescription _cardDescription;
        
        public bool? IsActive { get; private set; } = false;

        internal void Init(CardCapabilityDescription cardCapabilityDescription)
        {
            _bigCard.Init(cardCapabilityDescription);
            _capabilityDescription.Init(cardCapabilityDescription);
            _cardDescription.Init();
        }

        public void Activate(BigCardRootActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            if (data.CanActivateBigCard)
                _bigCard.Show(data.BigCardShowData);

            if (data.CanActivateCardDescription)
                _cardDescription.Show(data.CardDescriptionShowData);

            if (data.CanActivateCapabilityDescription)
                _capabilityDescription.Show(data.CapabilityDescriptionShowData);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            if (_bigCard.IsShown == true)
                _bigCard.Hide();

            if (_capabilityDescription.IsShown == true)
                _capabilityDescription.Hide();

            if (_cardDescription.IsShown == true)
                _cardDescription.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(BigCardRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineBigCard(),
                DefineCapabilityDescription(),
                DefineCardDescription()
            };

            return list;
        }

        [ContextMenu(nameof(DefineBigCard))]
        private ComponentAttachInfo DefineBigCard()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCapabilityDescription))]
        private ComponentAttachInfo DefineCapabilityDescription()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _capabilityDescription, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardDescription))]
        private ComponentAttachInfo DefineCardDescription()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardDescription, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}