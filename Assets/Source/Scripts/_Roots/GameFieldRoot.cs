using System.Collections.Generic;
using Cards;
using GameFields;
using GameFields.Decks;
using GameFields.Effects;
using GameFields.EndTurnButtons;
using GameFields.LightControls;
using GameFields.Persons;
using GameFields.Persons.Hands;
using GameFields.Seats;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;
using CanvasSortOrders;
using GameFields.InformationLabels;
using GameFields.Persons.LookCardMenues;
using GameFields.Persons.EffectHandlers.Brothers;
using GameFields.Persons.EffectHandlers;
using GameFields.InputSettings;
using GameFields.FightMenues;
using Cards.Views.BigCardViews.Capabilities;
using Cards.Views.BigCardViews;
using Cards.Sounds;
using GameFields.CardTransits;
using GameFields.Histories;
using Sounds;
using Tools;
using System;

namespace Roots
{
    public class GameFieldRoot : MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private CanvasRoot _canvasRoot;
        [SerializeField] private CardRoot _cardRoot;
        [SerializeField] private FontRoot _fontRoot;
        [SerializeField] private PersonCreator _personCreator;
        [SerializeField] private ObjectsLightControlsCreator _lightControlsCreator;
        [SerializeField] private SpeedUpButtonSortOrder _speedUpButtonSortOrder;
        [SerializeField] private GameField _fightPVE;

        private PersonsState _personsState;

        //private void Start()
        //{
        //    SceneContext sceneContext = FindObjectOfType<SceneContext>();
        //    sceneContext.Container.InjectGameObject(gameObject);

        //    Debug.Log("GameRoot: START");
        //}

        private SignalBus _bus;
        private Deck _deck;
        private SeatPool _seatPool;
        private BigCardRoot _bigCardRoot;
        private HandPlayer _handPlayer;
        private InformationLabel _informationLabel;
        private LookCardMenuPlayer _lookCardMenu;
        private VariantCardCreator _variantCardCreator;
        private SoundRoot _soundRoot;
        private CardSoundRoot _cardSoundRoot;
        private FightButtonsActivator _fightButtonsActivator;
        private CardCapabilityDescription _cardCapabilityDescription;
        private HistoryRoot _historyRoot;
        private BackgroundSoundConfig _backgroundSoundConfig;


        [Inject]
        private void Construct(SignalBus bus, Deck deck, SeatPool seatPool, BigCardRoot bigCardRoot, HandPlayer handPlayer,
            InformationLabel informationLabel, LookCardMenuPlayer lookCardMenu, VariantCardCreator variantCardCreator,
            SoundRoot soundRoot, CardSoundRoot cardSoundRoot, FightButtonsActivator fightButtonsActivator,
            CardCapabilityDescription cardCapabilityDescription, HistoryRoot historyRoot, BackgroundSoundConfig backgroundSoundConfig)
        {
            //StartCoroutine(Initing(bus, deck, seatPool, cardDescription, handPlayer, informationLabel, lookCardMenu, variantCardCreator, soundRoot,
            //    cardSoundVolume, fightMenuActivateButton, screenRoot));
            Debug.Log("GameRoot: CONSTRUCT");

            _bus = bus;
            _deck = deck;
            _seatPool = seatPool;
            _bigCardRoot = bigCardRoot;
            _handPlayer = handPlayer;
            _informationLabel = informationLabel;
            _lookCardMenu = lookCardMenu;
            _variantCardCreator = variantCardCreator;
            _soundRoot = soundRoot;
            _cardSoundRoot = cardSoundRoot;
            _fightButtonsActivator = fightButtonsActivator;
            _cardCapabilityDescription = cardCapabilityDescription;
            _historyRoot = historyRoot;
            _backgroundSoundConfig = backgroundSoundConfig;
        }

        //public void Init(bool isPVE)
        public void Init(Action onDestroyPrefab)
        {
            GameFieldGC.GCOFF();

            _canvasRoot.Init();
            _informationLabel.Init();
            _fontRoot.Init();

            _seatPool.Init();
            _endTurnButton.Init();

            _lightControlsCreator.Init();
            _speedUpButtonSortOrder.Init();

            _variantCardCreator.Init();
            _soundRoot.Init(_backgroundSoundConfig);

            CardDragAndDropLightController cardDragAndDropLightController = _lightControlsCreator.CreateCardDragAndDropLightController();

            //Destroy(_lightControlsCreator.gameObject);

            CardDragAndDropHandler cardDragAndDropHandler = new CardDragAndDropHandler(_handPlayer, _handPlayer,
                cardDragAndDropLightController, _speedUpButtonSortOrder);

            _personCreator.Init(_bus, _deck, _endTurnButton, _seatPool, cardDragAndDropHandler, cardDragAndDropLightController,
                _informationLabel, _cardRoot, _cardSoundRoot, _soundRoot, _cardCapabilityDescription, _historyRoot);

            Player player = _personCreator.CreatePlayer();
            EnemyAI enemyAI = _personCreator.CreateEnemyAI();
            CardLocationViewRoot viewRoot = _personCreator.CreateCardLocationViewRoot();
            CardTransitManager cardTransitManager = _personCreator.CreateCardTransitManager();
            BrothersEffectHandlerRoot brothersEffectHandlerRoot = _personCreator.CreateBrothersEffectHandlerRoot();
            PersonEffectsHandlerRoot personEffectsHandlerRoot = _personCreator.CreatePersonEffectsHandlerRoot();
            DiscardManager discardManager = _personCreator.DiscardManager;
            InputRoot inputRoot = _personCreator.GetInputRoot();
            LoseActionsRoot loseActionsRoot = _personCreator.CreateLoseActionsRoot();

            _lookCardMenu.Init(inputRoot);

            Destroy(_personCreator.gameObject);

            _personsState = new PersonsState(player, enemyAI);
            //fightMenu.
            ViewTransitTypesRoot typesRoot = new ViewTransitTypesRoot();
            EffectFactory effectFactory = new EffectFactory(_personsState, viewRoot, _informationLabel, cardTransitManager,
                _variantCardCreator, brothersEffectHandlerRoot, _bus, personEffectsHandlerRoot, discardManager, loseActionsRoot,
                _cardSoundRoot, typesRoot, _historyRoot);

            _cardRoot.Init(effectFactory, _bigCardRoot, cardDragAndDropHandler, _cardCapabilityDescription, _cardSoundRoot, _fontRoot);
            _deck.Init(_seatPool, _cardRoot.Cards);



            //IDeactivatable onMainMenuSwitcher = FindObjectOfType(typeof(Creator), true) as IDeactivatable;
            //IDeactivatable onMainMenuSwitcher = new TestFightRootDestroyer(() => Destroy(gameObject));

            _fightPVE.Init(_personsState, enemyAI, _bus, _seatPool, _soundRoot, _fightButtonsActivator,
    onDestroyPrefab);
            //_gameFieldRoot.Init(_personsState, enemyAI, _bus, _seatPool, _soundRoot, _fightButtonsActivator, onMainMenuSwitcher);
        }

        //private IEnumerator Initing(SignalBus bus, Deck deck, SeatPool seatPool, CardDescription cardDescription, HandPlayer handPlayer,
        //    InformationLabel informationLabel, LookCardMenuPlayer lookCardMenu, VariantC ardCreator variantCardCreator,
        //    SoundRoot soundRoot, CardSoundVolume cardSoundVolume, FightMenuActivateButton fightMenuActivateButton,
        //    ScreenRoot screenRoot)
        //{
        //    yield return new WaitForSeconds(0.1f); // Дадим сцене все прогрузить

        //    GameFieldGC.GCOFF();

        //    screenRoot.Init();
        //    informationLabel.Init();
        //    _fontRoot.Init();

        //    seatPool.Init();
        //    _endTurnButton.Init();

        //    _lightControlsCreator.Init();
        //    _speedUpButtonSortOrder.Init();

        //    variantCardCreator.Init();
        //    soundRoot.Init();

        //    CardDragAndDropLightController cardDragAndDropLightController = _lightControlsCreator.CreateCardDragAndDropLightController();

        //    //Destroy(_lightControlsCreator.gameObject);

        //    CardDragAndDropHandler cardDragAndDropHandler = new CardDragAndDropHandler(handPlayer, handPlayer,
        //        cardDragAndDropLightController, _speedUpButtonSortOrder);

        //    _personCreator.Init(bus, deck, _endTurnButton, seatPool, cardDragAndDropHandler, cardDragAndDropLightController,
        //        informationLabel, _cardRoot, cardSoundVolume, soundRoot);

        //    Player player = _personCreator.CreatePlayer();
        //    EnemyAI enemyAI = _personCreator.CreateEnemyAI();
        //    CardLocationViewRoot viewRoot = _personCreator.CreateCardLocationViewRoot();
        //    CardTransitManager cardTransitManager = _personCreator.CreateCardTransitManager();
        //    BrothersEffectHandlerRoot brothersEffectHandlerRoot = _personCreator.CreateBrothersEffectHandlerRoot();
        //    PersonEffectsHandlerRoot personEffectsHandlerRoot = _personCreator.CreatePersonEffectsHandlerRoot();
        //    DiscardManager discardManager = _personCreator.DiscardManager;
        //    InputRoot inputRoot = _personCreator.GetInputRoot();
        //    LoseActionsRoot loseActionsRoot = _personCreator.CreateLoseActionsRoot();

        //    lookCardMenu.Init(inputRoot);

        //    Destroy(_personCreator.gameObject);

        //    _personsState = new PersonsState(player, enemyAI);
        //    //fightMenu.

        //    EffectFactory effectFactory = new EffectFactory(_personsState, viewRoot, informationLabel, cardTransitManager,
        //        variantCardCreator, brothersEffectHandlerRoot, bus, personEffectsHandlerRoot, discardManager, loseActionsRoot);

        //    _cardRoot.Init(effectFactory, cardDescription, cardDragAndDropHandler, cardSoundVolume);
        //    deck.Init(seatPool, _cardRoot.Cards);

        //    _gameFieldRoot.Init(_personsState, enemyAI, bus, seatPool, soundRoot, fightMenuActivateButton);
        //}

        private bool _isSettedActionOnDestroy = false;
        private Action _onDestroy = null;

        public void SetActionOnDestroy(Action onDestroy)
        {
            if (_isSettedActionOnDestroy == false)
            {
                _onDestroy = onDestroy;
                _isSettedActionOnDestroy = true;
            }
        }

        public void OnDestroy()
        {
            GameFieldGC.GCON();
            _onDestroy?.Invoke();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineGameComponents))]
        private void DefineGameComponents()
        {
            List<ComponentAttachInfo> infos = new List<ComponentAttachInfo>();

            List<ComponentAttachInfo> exists = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> success = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> error = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> successButSoMuch = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> sceneNotExists = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> successForArray = new List<ComponentAttachInfo>();
            List<ComponentAttachInfo> noWay = new List<ComponentAttachInfo>();

            IAutomaticFillComponents[] gameComponents = GetComponentsInChildren<IAutomaticFillComponents>(true);
            int allComponents = 0;

            foreach (IAutomaticFillComponents component in gameComponents)
            {
                infos.AddRange(component.DefineAllComponents());
                allComponents++;
            }

            foreach (ComponentAttachInfo info in infos)
            {
                switch (info.ReturnValue)
                {
                    case 0:
                        exists.Add(info);
                        break;
                    case 1:
                        success.Add(info);
                        break;
                    case -1:
                        error.Add(info);
                        break;
                    case 2:
                        successButSoMuch.Add(info);
                        break;
                    case -2:
                        sceneNotExists.Add(info);
                        break;
                    case 3:
                        successForArray.Add(info);
                        break;
                    case -3:
                        noWay.Add(info);
                        break;
                    default:
                        throw new System.Exception("Неизвестный тип возвращаемого значения в ComponentAttachInfo: " + info.ReturnValue);
                }
            }

            Debug.Log($"Всего найдено {allComponents} компонентов");
            Debug.Log("------------------------------------------");
            ShowInfoByList("Уже заполнено", exists);
            ShowInfoByList("Успешно заполнены", success);
            ShowInfoByList("Произошла ошибка", error);
            ShowInfoByList("Заполнено, но возможно не то", successButSoMuch);
            ShowInfoByList("Нет на сцене", sceneNotExists);
            ShowInfoByList("Заполнены массивы", successForArray);
            ShowInfoByList("Сюда невозможно прийти", noWay);

            Debug.Log("ИТОГО:");
            Debug.Log("------------------------------------------");
            ShowResults("Уже заполнено", exists);
            ShowResults("Успешно заполнены", success);
            ShowResults("Произошла ошибка", error);
            ShowResults("Заполнено, но возможно не то", successButSoMuch);
            ShowResults("Нет на сцене", sceneNotExists);
            ShowResults("Заполнены массивы", successForArray);
            ShowResults("Сюда невозможно прийти", noWay);            //Debug.Log($"Удалось найти {gameComponents.Length} gameObject-ов. Из них автоматически заполнились: {allComponents}");
        }

        private void ShowResults(string allMessage, List<ComponentAttachInfo> currentList)
        {
            Debug.Log($"{allMessage}: {currentList.Count}:");
        }

        private void ShowInfoByList(string allMessage, List<ComponentAttachInfo> currentList)
        {
            Debug.Log($"{allMessage}: {currentList.Count}:");

            foreach (ComponentAttachInfo info in currentList)
            {
                Debug.Log(info.ComponentInfo);
            }

            Debug.Log("------------------------------------------");
        }

        [ContextMenu(nameof(DefineAllComponents) + nameof(GameFieldRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineEndTurnButton(),
                DefineCanvasRoot(),
                DefineCardRoot(),
                DefineFontRoot(),
                DefinePersonCreator(),
                DefineObjectsLightControlsCreator(),
                DefineFightPVE()
            };

            return list;
        }

        [ContextMenu(nameof(DefineEndTurnButton))]
        private ComponentAttachInfo DefineEndTurnButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _endTurnButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasRoot))]
        private ComponentAttachInfo DefineCanvasRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCardRoot))]
        private ComponentAttachInfo DefineCardRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFontRoot))]
        private ComponentAttachInfo DefineFontRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fontRoot, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefinePersonCreator))]
        private ComponentAttachInfo DefinePersonCreator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _personCreator, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineObjectsLightControlsCreator))]
        private ComponentAttachInfo DefineObjectsLightControlsCreator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lightControlsCreator, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightPVE))]
        private ComponentAttachInfo DefineFightPVE()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightPVE, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}