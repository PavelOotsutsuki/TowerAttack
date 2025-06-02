using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class CardRoot : MonoBehaviour, ICardWatcher, IAutomaticFillComponents
    {
        [SerializeField] private BigCardRoot _bigCardRoot;
        [SerializeField] private Card[] _cards;
        [SerializeField] private CurseAnimator _curseAnimator;

        private CardDescription _cardDescription;
        private CardViewService _cardViewService;
        private List<Card> _allCards;

        public IReadOnlyList<Card> Cards => _allCards;

        public void Init(IEffectFactory effectFactory, CardDescription cardDescription, ICardDragAndDropHandler cardDragAndDropHandler)
        {
            _cardDescription = cardDescription;
            _curseAnimator.Init();

            InitCardDescription();
            InitBigCard();

            _cardViewService = new CardViewService(_bigCardRoot, _cardDescription);

            InitCards(effectFactory, cardDragAndDropHandler);
        }

        private void InitCardDescription()
        {
            _cardDescription.Init();
        }

        private void InitBigCard()
        {
            _bigCardRoot.Init();
        }

        private void InitCards(IEffectFactory effectFactory, ICardDragAndDropHandler cardDragAndDropHandler)
        {
            _allCards = new List<Card>();

            foreach (Card card in _cards)
            {
                card.Init(effectFactory, _cardViewService, cardDragAndDropHandler, _curseAnimator);
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
                DefineBigCardRoot()
            };

            return list;
        }

        [ContextMenu(nameof(DefineAllCards))]
        private ComponentAttachInfo DefineAllCards()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _cards);
        }

        [ContextMenu(nameof(DefineBigCardRoot))]
        private ComponentAttachInfo DefineBigCardRoot()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _bigCardRoot, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}
