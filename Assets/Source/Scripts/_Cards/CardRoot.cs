using System;
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
using Cysharp.Threading.Tasks;
using Servers;
using Servers.DTO;
using Servers.Loaders;
using TMPro;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace Cards
{
    public class CardRoot : MonoBehaviour, ICardWatcher, ICardCreator, IAutomaticFillComponents
    {
        [SerializeField] private CurseAnimator _curseAnimator;
        [SerializeField] private CardCreator _cardCreator;
        [SerializeField] private StartCardsType _startCardsType;
        //[SerializeField] private CardDescription _cardDescription;
        [Inject] private FightProcessDBManager _fightProcessDBManager;
        [Inject] private DBRoot _dBRoot;
        [Inject] private AudioLoader _audioLoader;
        [Inject] private ImageLoader _imageLoader;

        private readonly List<Card> _allCards = new List<Card>();

        //private BigCardRoot _bigCardRoot;
        private CardViewService _cardViewService;

        private IEffectFactory _effectFactory;
        private ICardDragAndDropHandler _cardDragAndDropHandler;
        private CardCapabilityDescription _cardCapabilityDescription;
        private CardSoundRoot _cardSoundRoot;
        private IFontSetter _fontSetter;
        private CancellationToken _fightToken;
        private CardDescription _cardDescription;

        public IReadOnlyList<Card> Cards => _allCards;

        private string _allcardsMsg = "";

        public async UniTask Init(IEffectFactory effectFactory, ICardDragAndDropHandler cardDragAndDropHandler,
            CardCapabilityDescription cardCapabilityDescription, CardSoundRoot cardSoundRoot, IFontSetter fontSetter,
            CancellationToken fightToken, CardViewService cardViewService, CardDescription cardDescription)
        {
            _fightToken = fightToken;
            _cardDescription = cardDescription;

            _curseAnimator.Init(_fightToken);
            _cardCreator.Init();

            //_bigCardRoot = bigCardRoot;
            //_bigCardRoot.Init(cardCapabilityDescription, _cardDescription);

            _effectFactory = effectFactory;
            _cardDragAndDropHandler = cardDragAndDropHandler;
            _cardCapabilityDescription = cardCapabilityDescription;
            _cardSoundRoot = cardSoundRoot;
            _fontSetter = fontSetter;

            //_cardViewService = new CardViewService(_bigCardRoot);
            _cardViewService = cardViewService;

            if (_startCardsType != StartCardsType.DB)
            {
                StartCards startCards = new StartCards(_startCardsType);

                foreach (CardName cardName in startCards.StartCardNames)
                {
                    await CreateCard(cardName, transform, _fightToken);
                }

                Debug.Log(_allcardsMsg);
            }
            else
            {

            }
        }

        public Card CreateCard(CardName cardName, Transform parent)
        {
            Card createdCard = _cardCreator.CreateInstantly(cardName, parent);
            createdCard.Init(_effectFactory, _cardViewService, _cardDragAndDropHandler, _curseAnimator,
                _cardCapabilityDescription, _cardSoundRoot, _cardDescription, _fightToken, _dBRoot, null, null);

            _allcardsMsg += $"{createdCard.ViewData.Number}: {(int)createdCard.CardPersonType}\n";
            _fightProcessDBManager.WriteFightProcessAction(-1, null, createdCard.LogID, "CREATE", "CARD");

            TMP_Text[] cardTexts = createdCard.gameObject.GetComponentsInChildren<TMP_Text>(true);
            _fontSetter.SetFont(cardTexts);

            _allCards.Add(createdCard);

            return createdCard;
        }

        public async UniTask<Card> CreateCard(CardName cardName, Transform parent, CancellationToken token)
        {
            Card createdCard = _cardCreator.CreateInstantly(cardName, parent);

            try
            {
                string baseUrl = _cardCreator.GetAudioUrl(cardName);

                AudioClip[] clips = await _audioLoader.GetClipsAsync(new StringIntPair {key = baseUrl, value = -1}, token);
                Sprite icon = await _imageLoader.GetImageAsync(baseUrl, token);  

                CardSoundConfigDB cardSoundConfigDB = new CardSoundConfigDB(clips);
                CardViewConfigDB cardViewConfigDB = new CardViewConfigDB(icon);

                createdCard.Init(_effectFactory, _cardViewService, _cardDragAndDropHandler, _curseAnimator,
                    _cardCapabilityDescription, _cardSoundRoot, _cardDescription, _fightToken, _dBRoot, cardSoundConfigDB, cardViewConfigDB);

                _allcardsMsg += $"{createdCard.ViewData.Number}: {(int)createdCard.CardPersonType}\n";
                _fightProcessDBManager.WriteFightProcessAction(-1, null, createdCard.LogID, "CREATE", "CARD");

                TMP_Text[] cardTexts = createdCard.gameObject.GetComponentsInChildren<TMP_Text>(true);
                _fontSetter.SetFont(cardTexts);
            }
            catch (Exception ex)
            {
                Debug.Log($"Exception {nameof(CreateCard)}: {ex.Message}");
            }

            _allCards.Add(createdCard);

            return createdCard;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(CardRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                //DefineBigCardRoot(),
                DefineCardCreator(),
                //DefineCardDescription()
            };

            return list;
        }

        //[ContextMenu(nameof(DefineBigCardRoot))]
        //private ComponentAttachInfo DefineBigCardRoot()
        //{
        //   return AutomaticFillComponents.DefineComponent(this, ref _bigCardRoot, ComponentLocationTypes.InChildren);
        //}

        [ContextMenu(nameof(DefineCardCreator))]
        private ComponentAttachInfo DefineCardCreator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardCreator, ComponentLocationTypes.InThis);
        }

        //[ContextMenu(nameof(DefineCardDescription))]
        //private ComponentAttachInfo DefineCardDescription()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _cardDescription, ComponentLocationTypes.InScene);
        //}
        #endregion
    }
}