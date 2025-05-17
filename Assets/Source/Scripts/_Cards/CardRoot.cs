using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class CardRoot : MonoBehaviour, ICardWatcher, IAutomaticFillComponents
    {
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private Card[] _cards;

        private CardDescription _cardDescription;
        private CardViewService _cardViewService;
        private List<Card> _allCards;

        public IReadOnlyList<Card> Cards => _allCards;

        public void Init(IEffectFactory effectFactory, CardDescription cardDescription, ICardDragAndDropHandler cardDragAndDropHandler)
        {
            _cardDescription = cardDescription;

            InitCardDescription();
            InitBigCard();

            _cardViewService = new CardViewService(_bigCard, _cardDescription);

            InitCards(effectFactory, cardDragAndDropHandler);
        }

        private void InitCardDescription()
        {
            _cardDescription.Init();
        }

        private void InitBigCard()
        {
            _bigCard.Init();
        }

        private void InitCards(IEffectFactory effectFactory, ICardDragAndDropHandler cardDragAndDropHandler)
        {
            _allCards = new List<Card>();

            foreach (Card card in _cards)
            {
                card.Init(effectFactory, _cardViewService, cardDragAndDropHandler);
                _allCards.Add(card);
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
