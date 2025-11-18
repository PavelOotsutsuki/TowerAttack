using System.Collections.Generic;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class CardRoot : MonoBehaviour, ICardWatcher, IAutomaticFillComponents
    {
        [SerializeField] private Card[] _startCards;
        [SerializeField] private CurseAnimator _curseAnimator;

        private readonly List<Card> _allCards = new List<Card>();

        private BigCardRoot _bigCardRoot;
        private CardViewService _cardViewService;

        public IReadOnlyList<Card> Cards => _allCards;

        public void Init(IEffectFactory effectFactory, BigCardRoot bigCardRoot, ICardDragAndDropHandler cardDragAndDropHandler,
            CardSoundVolume cardSoundVolume, CardCapabilityDescription cardCapabilityDescription)
        {
            _bigCardRoot = bigCardRoot;
            _curseAnimator.Init();

            _bigCardRoot.Init(cardCapabilityDescription);

            _cardViewService = new CardViewService(_bigCardRoot);

            foreach (Card card in _startCards)
            {
                card.Init(effectFactory, _cardViewService, cardDragAndDropHandler, _curseAnimator, cardSoundVolume,
                    cardCapabilityDescription);
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
           return AutomaticFillComponents.DefineComponent(this, ref _startCards);
        }

        [ContextMenu(nameof(DefineBigCardRoot))]
        private ComponentAttachInfo DefineBigCardRoot()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _bigCardRoot, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}