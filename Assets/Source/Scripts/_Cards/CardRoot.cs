using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class CardRoot : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private Card[] _cards;
        [SerializeField] private Transform _dragContainer;

        private CardDescription _cardDescription;
        private CardViewService _cardViewService;

        public IEnumerable<Card> Cards => _cards;

        public void Init(IEffectFactory effectFactory, CardDescription cardDescription)
        {
            _cardDescription = cardDescription;

            InitCardDescription();
            InitBigCard();

            _cardViewService = new CardViewService(_bigCard, _cardDescription);

            InitCards(effectFactory);
        }

        private void InitCardDescription()
        {
            _cardDescription.Init();
        }

        private void InitBigCard()
        {
            _bigCard.Init();
        }

        private void InitCards(IEffectFactory effectFactory)
        {
            foreach (Card card in _cards)
            {
                card.Init(effectFactory, _cardViewService, _dragContainer);
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAllCards(),
                DefineBigCard()
            };

            return list;
        }

        [ContextMenu(nameof(DefineAllCards))]
        private ComponentAttachInfo DefineAllCards()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _cards);
        }

        [ContextMenu(nameof(DefineBigCard))]
        private ComponentAttachInfo DefineBigCard()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
