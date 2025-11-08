using System.Collections;
using System.Collections.Generic;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class BigCardRoot : MonoBehaviour, IWorkable<BigCardRootActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private BigCardDescription _description;
        [SerializeField] private CardDescription _cardDescription;

        private readonly CardCapabilityDescription _cardCapabilityDescription = new CardCapabilityDescription();

        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            _bigCard.Init();
            _description.Init();
            _cardDescription.Init();
        }

        public void Activate(BigCardRootActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _bigCard.Show(data.BigCardShowData);
            _cardDescription.Show(data.BigCardShowData.LabelData);

            //string cardCapabilityDescription = _cardCapabilityDescription.GetDescription(data.BigCardShowData.CardViewData.CardCapability);
            string cardCapabilityDescription = _cardCapabilityDescription.GetToStringValue(data.BigCardShowData.CardViewData.CardCapability);

            if (string.IsNullOrWhiteSpace(cardCapabilityDescription) == false)
            {
                LabelActivateData labelActivateData = new LabelActivateData(cardCapabilityDescription);

                _description.Show(labelActivateData);
            }
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _bigCard.Hide();
            _description.Hide();
            _cardDescription.Hide();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(BigCardRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineBigCard(),
                DefineBigCardDescription()
            };

            return list;
        }

        [ContextMenu(nameof(DefineBigCard))]
        private ComponentAttachInfo DefineBigCard()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineBigCardDescription))]
        private ComponentAttachInfo DefineBigCardDescription()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _description, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}
