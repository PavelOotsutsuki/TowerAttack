using System.Collections.Generic;
using Cards.Animations.Curses;
using Cards.Effects;
using Cards.Insides;
using Cards.Sounds;
using Cards.Views;
using Cards.Views.BigCardViews;
using Cards.Views.BigCardViews.Capabilities;
using TMPro;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class CardRoot : MonoBehaviour, ICardWatcher, ICardCreator, IAutomaticFillComponents
    {
        //[SerializeField] private Card[] _startCards;
        [SerializeField] private CurseAnimator _curseAnimator;
        [SerializeField] private CardCreator _cardCreator;
        [SerializeField] private StartCardsType _startCardsType;
        [SerializeField] private UIHelper[] _uIHelpers;

        private readonly List<Card> _allCards = new List<Card>();

        private BigCardRoot _bigCardRoot;
        private CardViewService _cardViewService;

        private IEffectFactory _effectFactory;
        private ICardDragAndDropHandler _cardDragAndDropHandler;
        private CardCapabilityDescription _cardCapabilityDescription;
        private CardSoundRoot _cardSoundRoot;
        private IFontSetter _fontSetter;

        public IReadOnlyList<Card> Cards => _allCards;

        public void Init(IEffectFactory effectFactory, BigCardRoot bigCardRoot, ICardDragAndDropHandler cardDragAndDropHandler,
            CardCapabilityDescription cardCapabilityDescription, CardSoundRoot cardSoundRoot, IFontSetter fontSetter)
        {
            _curseAnimator.Init();
            _cardCreator.Init();

            _bigCardRoot = bigCardRoot;
            _bigCardRoot.Init(cardCapabilityDescription);

            _effectFactory = effectFactory;
            _cardDragAndDropHandler = cardDragAndDropHandler;
            _cardCapabilityDescription = cardCapabilityDescription;
            _cardSoundRoot = cardSoundRoot;
            _fontSetter = fontSetter;

            _cardViewService = new CardViewService(_bigCardRoot);
            StartCards startCards = new StartCards(_startCardsType);

            foreach (CardName cardName in startCards.StartCardNames)
            {
                CreateCard(cardName, transform);
            }
        }

        public Card CreateCard(CardName cardName, Transform parent)
        {
            Card createdCard = _cardCreator.CreateInstantly(cardName, parent);

            createdCard.Init(_effectFactory, _cardViewService, _cardDragAndDropHandler, _curseAnimator,
                _cardCapabilityDescription, _cardSoundRoot);

            TMP_Text[] cardTexts = createdCard.gameObject.GetComponentsInChildren<TMP_Text>(true);
            _fontSetter.SetFont(cardTexts);

            _allCards.Add(createdCard);

            return createdCard;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                //DefineAllCards(),
                DefineBigCardRoot(),
                DefineCardCreator()
            };

            return list;
        }

        //[ContextMenu(nameof(DefineAllCards))]
        //private ComponentAttachInfo DefineAllCards()
        //{
        //   return AutomaticFillComponents.DefineComponent(this, ref _startCards);
        //}

        [ContextMenu(nameof(DefineBigCardRoot))]
        private ComponentAttachInfo DefineBigCardRoot()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _bigCardRoot, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardCreator))]
        private ComponentAttachInfo DefineCardCreator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardCreator, ComponentLocationTypes.InThis);
        }


        [ContextMenu(nameof(DefineUIHelpers))]
        private ComponentAttachInfo DefineUIHelpers()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _uIHelpers, true);
        }
        #endregion 
    }
}