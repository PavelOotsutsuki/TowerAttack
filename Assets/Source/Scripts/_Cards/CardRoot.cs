using System.Collections.Generic;
using System.Threading;
using Cards.Animations.Curses;
using Cards.Effects;
using Cards.Insides;
using Cards.Sounds;
using Cards.Views;
using Cards.Views.BigCardViews;
using Cards.Views.BigCardViews.Capabilities;
using Cards.Views.BigCardViews.CardDescriptions;
using TMPro;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace Cards
{
    public class CardRoot : MonoBehaviour, ICardWatcher, ICardCreator, IAutomaticFillComponents
    {
        [SerializeField] private CurseAnimator _curseAnimator;
        [SerializeField] private CardCreator _cardCreator;
        [SerializeField] private StartCardsType _startCardsType;
        [SerializeField] private CardDescription _cardDescription;

        private readonly List<Card> _allCards = new List<Card>();

        private BigCardRoot _bigCardRoot;
        private CardViewService _cardViewService;

        private IEffectFactory _effectFactory;
        private ICardDragAndDropHandler _cardDragAndDropHandler;
        private CardCapabilityDescription _cardCapabilityDescription;
        private CardSoundRoot _cardSoundRoot;
        private IFontSetter _fontSetter;
        private CancellationToken _fightToken;

        public IReadOnlyList<Card> Cards => _allCards;

        public void Init(IEffectFactory effectFactory, BigCardRoot bigCardRoot, ICardDragAndDropHandler cardDragAndDropHandler,
            CardCapabilityDescription cardCapabilityDescription, CardSoundRoot cardSoundRoot, IFontSetter fontSetter,
            CancellationToken fightToken)
        {
            _fightToken = fightToken;

            _curseAnimator.Init(_fightToken);
            _cardCreator.Init();

            _bigCardRoot = bigCardRoot;
            _cardDescription.Init();
            _bigCardRoot.Init(cardCapabilityDescription, _cardDescription);

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
                _cardCapabilityDescription, _cardSoundRoot, _cardDescription, _fightToken);

            TMP_Text[] cardTexts = createdCard.gameObject.GetComponentsInChildren<TMP_Text>(true);
            _fontSetter.SetFont(cardTexts);

            _allCards.Add(createdCard);

            return createdCard;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineBigCardRoot(),
                DefineCardCreator(),
                DefineCardDescription()
            };

            return list;
        }

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

        [ContextMenu(nameof(DefineCardDescription))]
        private ComponentAttachInfo DefineCardDescription()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardDescription, ComponentLocationTypes.InScene);
        }
        #endregion 
    }
}