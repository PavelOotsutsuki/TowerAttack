using System.Collections.Generic;
using GameFields.Decks;
using GameFields.EndTurnButtons;
using GameFields.LightControls;
using GameFields.Persons.SelectMenues.Attacks;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;
using GameFields.Persons.SelectMenues;
using GameFields.Persons.SelectMenues.Choices;
using GameFields.InformationLabels;
using GameFields.DiscardPiles;
using GameFields.Persons.Fires;
using Cards;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.EffectHandlers.Slimes;
using Tools.Settings;
using GameFields.Persons.EffectHandlers.Curses;
using GameFields.Persons.EnemyProcessImitations;
using GameFields.Persons.EffectHandlers.Fires;
using GameFields.Persons.LookCardMenues;
using GameFields.Persons.EffectHandlers.Brothers;
using GameFields.Persons.EffectHandlers.Scarecrows;
using GameFields.InputSettings;
using GameFields.FightMenues;
using Cards.Views.BigCardViews.Capabilities;
using GameFields.Persons.EffectHandlers.FateInevitabilities;
using Cards.Sounds;
using Cards.Views;
using GameFields.CardTransits;
using Tools.UI;
using Tools.UI.UIHelpers;
using GameFields.Histories;
using GameFields.Persons.ConfirmableNumbersView;
using System.Threading;

namespace GameFields.Persons
{
    public class PersonCreator : MonoBehaviour, IAutomaticFillComponents
    {
        private readonly int[] _cardNumbers = GameSettings.DefaultCardNumbers;

        [Header("Player Fields:")]

        private CardPlayingZonePlayer _playerPlayingZone;
        private HandPlayer _playerHand;
        private Table _playerTable;
        private TowerPlayer _playerTower;
        private DiscoverPlayer _playerDiscover;
        private ChoiceMenuPlayer _playerChoiceMenu;
        private ChoiceMenuImitationPlayer _playerChoiceMenuImitation;
        private AttackMenuPlayer _playerAttackMenu;
        private CardAttackZonePlayer _playerCardAttackZone;
        private FirePoolPlayer _playerFirePool;
        private RechangeFeatureRuleController _playerRechangeFeatureRuleController;
        private TurnDrawnCards _playerTurnDrawnCards;
        private PersonEffectsHandler _playerPersonEffectsHandler;

        private LookCardMenuPlayer _playerLookCardMenu;
        private FightMenu _fightMenu;
        private FightMenuActivateButton _fightMenuActivateButton;
        private HistoryMenu _historyMenu;
        private HistoryMenuActivateButton _historyMenuActivateButton;
        private FightButtonsActivator _fightButtonsActivator;
        //private ActiveEffectsList _playerActiveEffectsList = new ActiveEffectsList();

        private ForgingZone _forgingZone;
        private HandTransferZone _handTransferZone;

        private SelectNumbersList _attackedNumbersPlayer = new SelectNumbersList();
        private SelectNumbersList _choicedNumbersPlayer = new SelectNumbersList();
        private SelectNumbersList _cursedNumbersPlayer = new SelectNumbersList();
        private ConfirmableNumbers _confirmableNumbersPlayer;
        private LastSelectedNumbersWatcher _lastSelectedNumbersWatcherPlayer = new LastSelectedNumbersWatcher();
        private PersonEffectKeeper _playerEffectKeeper;

        private BrothersEffectHandler _playerBrothersEffectHandler;

        private LoseActions _playerLoseActions;

        private StartPlayerTurnLabel _startPlayerTurnLabel;

        private SkipTurnEffectHandler _skipTurnEffectHandlerPlayer;
        private FateInevitabilityHandler _fateInevitabilityHandlerPlayer;
        //private ScarecrowEffectHandler _scarecrowEffectHandlerPlayer;
        //private WiseMonkEffectHandler _wiseMonkEffectHandlerPlayer;

        private DrawCardRoot _drawCardRootPlayer;
        private FireEffectHandler _fireEffectHandlerPlayer;

        [SerializeField] private int _playerCountStartDrawCards = 1;
        [SerializeField] private AttackResultHandlerData _attackResultHandlerPlayerData;
        [SerializeField] private InformationLabelData _informationLabelDataPlayerChoice;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("EnemyAI Fields:")]

        [SerializeField] private EnemyDragAndDropImitationData _enemyDragAndDropImitationData;
        [SerializeField] private AttackResultHandlerData _attackResultHandlerEnemyAIData;
        [SerializeField] private InformationLabelData _informationLabelDataEnemyAIAttack;
        [SerializeField] private InformationLabelData _informationLabelDataEnemyAIChoice;

        private CardPlayingZone _enemyPlayingZone;
        private HandAI _enemyHand;
        private Table _enemyTable;
        private TowerAI _enemyTower;
        private DiscoverAI _enemyDiscoverImitation;
        private ChoiceMenuEnemyAI _enemyChoiceMenu;
        private ChoiceMenuImitationEnemyAI _enemyChoiceMenuImitation;
        private AttackMenuEnemyAI _enemyAttackMenu;
        private CardAttackZoneEnemyAI _enemyCardAttackZone;
        private FirePoolEnemy _enemyFirePool;
        private RechangeFeatureRuleController _enemyRechangeFeatureRuleController;
        private TurnDrawnCards _enemyTurnDrawnCards;
        private PersonEffectsHandler _enemyPersonEffectsHandler;

        //private ActiveEffectsList _enemyActiveEffectsList = new ActiveEffectsList();

        private SelectNumbersList _attackedNumbersEnemy = new SelectNumbersList();
        private SelectNumbersList _choicedNumbersEnemy = new SelectNumbersList();
        private SelectNumbersList _cursedNumbersEnemyAI = new SelectNumbersList();
        private ConfirmableNumbers _confirmableNumbersEnemyAI;
        private LastSelectedNumbersWatcher _lastSelectedNumbersWatcherEnemyAI = new LastSelectedNumbersWatcher();
        private PersonEffectKeeper _enemyEffectKeeper;

        private BrothersEffectHandler _enemyBrothersEffectHandler;

        private LoseActions _enemyLoseActions;

        private DrawCardRoot _drawCardRootEnemy;
        private FireEffectHandler _fireEffectHandlerEnemy;

        [SerializeField] private int _enemyCountStartDrawCards = 1;
        
        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("GameFieldObjectsActivator:")]

        //[SerializeField] private TableActivator _tableActivator;
        //private TableActivator _tableActivator;
        [SerializeField] private TowerActivator _towerActivator;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("Draw Card Animation's data:")]

        [SerializeField] private SimpleDrawCardAnimationData _playerSimpleDrawCardAnimationData;
        [SerializeField] private SimpleDrawCardAnimationData _enemyAISimpleDrawCardAnimationData;
        [SerializeField] private FireDrawCardAnimationData _playerFireDrawCardAnimationData;
        [SerializeField] private FireDrawCardAnimationData _enemyAIFireDrawCardAnimationData;
        [SerializeField] private FireCardContainer _fireContainer;
        //[SerializeField] private float _fireDrawCardDelay = 2f;
        //[SerializeField] private InvertCardAnimationData _fireAnimationInvertData;

        private SignalBus _bus;
        private Deck _deck;
        private EndTurnButton _endTurnButton;
        private SeatPool _seatPool;
        private DiscardManager _discardManager;
        private SkipTurnLabelPlayer _skipTurnLabelPlayer;
        private SkipTurnLabelEnemyAI _skipTurnLabelEnemyAI;
        private GameFieldInputRoot _inputRoot;

        private InteractionActivator _interactionActivator;
        private InformationLabel _informationLabel;
        private DiscardPile _discardPile;
        private FireRoot _fireRoot;
        private CardRoot _cardRoot;
        private UIHelperDescription _UIHelperDescription;
        private HistoryRoot _historyRoot;

        private CancellationToken _gameFieldToken;
        private CancellationToken _fightToken;

        private ConfirmableNumbersViewRoot _confirmableNumbersViewRoot;

        public DiscardManager DiscardManager => _discardManager;

        [Inject]
        public void Construct(CardPlayingZonePlayer playerPlayingZone, HandPlayer playerHand, TablePlayer playerTable, TowerPlayer playerTower,
            DiscoverPlayer playerDiscover, AttackMenuPlayer playerAttackMenu, CardPlayingZoneAI enemyPlayingZone, HandAI enemyHand,
            TableAI enemyTable, TowerAI enemyTower, DiscoverAI enemyDiscoverImitation, AttackMenuEnemyAI enemyAttackMenu,
            CardAttackZonePlayer playerCardAttackZone, CardAttackZoneEnemyAI enemyCardAttackZone, ChoiceMenuPlayer playerChoiceMenu,
            ChoiceMenuEnemyAI enemyChoiceMenu, DiscardPile discardPile, ChoiceMenuImitationPlayer choiceMenuImitationPlayer,
            ChoiceMenuImitationEnemyAI choiceMenuImitationEnemyAI, ForgingZone forgingZone, HandTransferZone handTransferZone,
            LookCardMenuPlayer lookCardMenuPlayer, StartPlayerTurnLabel startPlayerTurnLabel, SkipTurnLabelPlayer skipTurnLabelPlayer,
            SkipTurnLabelEnemyAI skipTurnLabelEnemyAI, FightMenu fightMenu, FightMenuActivateButton fightMenuActivateButton,
            UIHelperDescription UIHelperDescription, HistoryMenu historyMenu, HistoryMenuActivateButton historyMenuActivateButton,
            FightButtonsActivator fightButtonsActivator, ConfirmableNumbersViewRoot confirmableNumbersViewRoot)
        {
            _playerPlayingZone = playerPlayingZone;
            _playerHand = playerHand;
            _playerTable = playerTable;
            _playerTower = playerTower;
            _playerDiscover = playerDiscover;
            _playerChoiceMenu = playerChoiceMenu;
            _playerChoiceMenuImitation = choiceMenuImitationPlayer;
            _playerAttackMenu = playerAttackMenu;
            _playerCardAttackZone = playerCardAttackZone;

            _playerLookCardMenu = lookCardMenuPlayer;
            _startPlayerTurnLabel = startPlayerTurnLabel;

            _forgingZone = forgingZone;
            _handTransferZone = handTransferZone;

            _enemyPlayingZone = enemyPlayingZone;
            _enemyHand = enemyHand;
            _enemyTable = enemyTable;
            _enemyTower = enemyTower;
            _enemyDiscoverImitation = enemyDiscoverImitation;
            _enemyChoiceMenu = enemyChoiceMenu;
            _enemyChoiceMenuImitation = choiceMenuImitationEnemyAI;
            _enemyAttackMenu = enemyAttackMenu;
            _enemyCardAttackZone = enemyCardAttackZone;

            _discardPile = discardPile;
            _skipTurnLabelPlayer = skipTurnLabelPlayer;
            _skipTurnLabelEnemyAI = skipTurnLabelEnemyAI;

            _fightMenu = fightMenu;
            _fightMenuActivateButton = fightMenuActivateButton;

            _historyMenu = historyMenu;
            _historyMenuActivateButton = historyMenuActivateButton;

            _fightButtonsActivator = fightButtonsActivator;

            _UIHelperDescription = UIHelperDescription;
            _confirmableNumbersViewRoot = confirmableNumbersViewRoot;
            //_inputRoot = inputRoot;
        }

        public void Init(SignalBus bus, Deck deck, EndTurnButton endTurnButton, SeatPool seatPool,
            CardDragAndDropHandler cardDragAndDropHandler, CardDragAndDropLightController cardDragAndDropLightController,
            InformationLabel informationLabel, CardRoot cardRoot, CardSoundRoot cardSoundRoot, IVolume musicVolume,
            CardCapabilityDescription cardCapabilityDescription, HistoryRoot historyRoot, ISoundController soundController,
            CancellationToken gameFieldToken, CancellationToken fightToken)
        {
            _bus = bus;
            _deck = deck;
            _endTurnButton = endTurnButton;
            _seatPool = seatPool;
            _informationLabel = informationLabel;
            _cardRoot = cardRoot;
            _historyRoot = historyRoot;

            _gameFieldToken = gameFieldToken;
            _fightToken = fightToken;

            _skipTurnLabelPlayer.Init();
            _skipTurnLabelEnemyAI.Init();
            _inputRoot = new GameFieldInputRoot(_endTurnButton, _fightMenu, _fightMenu, _historyMenu);

            _enemyLoseActions = new LoseActions(_enemyTower, _enemyTower, _playerHand, _bus, _inputRoot, _fightMenu,
                _fightButtonsActivator, soundController);
            _playerLoseActions = new LoseActions(_playerTower, _playerTower, _playerHand, _bus, _inputRoot, _fightMenu,
                _fightButtonsActivator, soundController);

            _fightMenu.Init(_inputRoot, _playerLoseActions, cardSoundRoot, musicVolume, cardCapabilityDescription, fightToken);
            _fightMenuActivateButton.Init(_fightMenu, _UIHelperDescription);

            _historyMenu.Init(historyRoot);
            _historyMenuActivateButton.Init(_historyMenu, _UIHelperDescription);

            DefineFire();

            _playerRechangeFeatureRuleController = new RechangeFeatureRuleController();
            _enemyRechangeFeatureRuleController = new RechangeFeatureRuleController();

            TableActivator tableActivator = new TableActivator(_playerPlayingZone);
            _interactionActivator = new InteractionActivator(cardDragAndDropHandler, _towerActivator, tableActivator, endTurnButton,
                cardDragAndDropLightController, _forgingZone, _handTransferZone, _inputRoot);

            _confirmableNumbersPlayer = new ConfirmableNumbers(_attackedNumbersPlayer, _choicedNumbersPlayer, _cursedNumbersPlayer);
            _confirmableNumbersEnemyAI = new ConfirmableNumbers(_attackedNumbersEnemy, _choicedNumbersEnemy, _cursedNumbersEnemyAI);

            _confirmableNumbersViewRoot.Init(_confirmableNumbersEnemyAI, _confirmableNumbersPlayer, _fightToken);

            _discardManager = new DiscardManager(_enemyTable, _playerTable);

            _playerEffectKeeper = new PersonEffectKeeper();
            _enemyEffectKeeper = new PersonEffectKeeper();

            InitPlayersData();
            InitEnemyData();
            //InitCommonData();

        }

        public Player CreatePlayer()
        {
            SlimeEffectHandler slimeEffectHandler = new SlimeEffectHandler(_playerHand, _playerTurnDrawnCards);
            List<ICardFeatureRechangablePlace> cardFeatureRechangables = new List<ICardFeatureRechangablePlace>()
            {
                _playerTower,
                _playerHand
            };
            GnomeEffectHandler gnomeEffectHandler = new GnomeEffectHandler(_playerRechangeFeatureRuleController, cardFeatureRechangables);
            CurseEffectHandlerPlayer curseEffectHandler = new CurseEffectHandlerPlayer(_playerTower, _informationLabel, _confirmableNumbersEnemyAI,
                _cursedNumbersEnemyAI, _bus);
            DoubleEffectHandler doubleEffectHandler = new DoubleEffectHandler();
            JusticeBullEffectHandler justiceBullEffectHandler = new JusticeBullEffectHandler(_choicedNumbersPlayer, _enemyTower);
            ScarecrowEffectHandler scarecrowEffectHandler = new ScarecrowEffectHandler(_discardManager);
            WiseMonkEffectHandler wiseMonkEffectHandler = new WiseMonkEffectHandler();
            FalsePrinceEffectHandler falsePrinceEffectHandler = new FalsePrinceEffectHandler(_playerTower, _deck);
            FallenGuardianEffectHandler fallenGuardianEffectHandler = new FallenGuardianEffectHandler(_choicedNumbersPlayer, _enemyTower);
            _playerBrothersEffectHandler = new BrothersEffectHandler(_playerRechangeFeatureRuleController, cardFeatureRechangables);
            _playerPersonEffectsHandler = new PersonEffectsHandler(gnomeEffectHandler, slimeEffectHandler, curseEffectHandler,
                _fireEffectHandlerPlayer, doubleEffectHandler, _skipTurnEffectHandlerPlayer, _fateInevitabilityHandlerPlayer, justiceBullEffectHandler,
                _playerBrothersEffectHandler, scarecrowEffectHandler, wiseMonkEffectHandler, falsePrinceEffectHandler,
                fallenGuardianEffectHandler);

            SkipTurnChecker skipTurnChecker = new SkipTurnChecker(slimeEffectHandler, _playerHand);
            TurnProcessingCreator turnProcessingCreator = new TurnProcessingCreator(_interactionActivator, skipTurnChecker);
            StartTurnDrawPlayerCreator startTurnDraw = new StartTurnDrawPlayerCreator(_interactionActivator, _drawCardRootPlayer, _playerCountStartDrawCards);

            StartPlayerTurnViewCreator startPlayerTurnViewCreator = new StartPlayerTurnViewCreator(_interactionActivator, _startPlayerTurnLabel);
            PlayerSkipTurnViewCreator skipTurnView = new PlayerSkipTurnViewCreator(_interactionActivator, _skipTurnLabelPlayer);
            EndTurnProcessingCreator endTurnProcessingCreator = new EndTurnProcessingCreator(_endTurnButton, _interactionActivator, _playerPersonEffectsHandler);

            _forgingZone.Init(_discardPile, _bus, _drawCardRootPlayer, gnomeEffectHandler, _historyRoot);
            _handTransferZone.Init(_enemyHand, _bus, _historyRoot);

            _playerHand.Init(_seatPool, _playerRechangeFeatureRuleController, _playerTurnDrawnCards, curseEffectHandler);

            return new Player(_interactionActivator, _playerHand, _playerPlayingZone, _playerTower, _playerDiscover,
                _drawCardRootPlayer, startTurnDraw, turnProcessingCreator, _bus, startPlayerTurnViewCreator, _playerAttackMenu, endTurnProcessingCreator,
                _playerChoiceMenu, _playerChoiceMenuImitation, _playerPersonEffectsHandler, _informationLabel, _playerLookCardMenu,
                skipTurnView, _confirmableNumbersPlayer, _lastSelectedNumbersWatcherPlayer, _playerEffectKeeper, _gameFieldToken);
        }

        public EnemyAI CreateEnemyAI()
        {
            SlimeEffectHandler slimeEffectHandler = new SlimeEffectHandler(_enemyHand, _enemyTurnDrawnCards);
            List<ICardFeatureRechangablePlace> cardFeatureRechangables = new List<ICardFeatureRechangablePlace>()
            {
                _enemyTower,
                _enemyHand
            };
            GnomeEffectHandler gnomeEffectHandler = new GnomeEffectHandler(_enemyRechangeFeatureRuleController, cardFeatureRechangables);
            CurseEffectHandlerEnemyAI curseEffectHandler = new CurseEffectHandlerEnemyAI(_enemyTower, _informationLabel, _confirmableNumbersPlayer,
                _cursedNumbersPlayer, _bus);
            DoubleEffectHandler doubleEffectHandler = new DoubleEffectHandler();
            SkipTurnEffectHandler skipTurnEffectHandler = new SkipTurnEffectHandler();
            FateInevitabilityHandler fateInevitabilityHandler = new FateInevitabilityHandler(_enemyLoseActions, _enemyAttackMenu);
            JusticeBullEffectHandler justiceBullEffectHandler = new JusticeBullEffectHandler(_choicedNumbersEnemy, _playerTower);
            _enemyBrothersEffectHandler = new BrothersEffectHandler(_enemyRechangeFeatureRuleController, cardFeatureRechangables);
            ScarecrowEffectHandler scarecrowEffectHandler = new ScarecrowEffectHandler(_discardManager);
            WiseMonkEffectHandler wiseMonkEffectHandler = new WiseMonkEffectHandler();
            FalsePrinceEffectHandler falsePrinceEffectHandler = new FalsePrinceEffectHandler(_enemyTower, _deck);
            FallenGuardianEffectHandler fallenGuardianEffectHandler = new FallenGuardianEffectHandler(_choicedNumbersEnemy, _playerTower);
            _enemyPersonEffectsHandler = new PersonEffectsHandler(gnomeEffectHandler, slimeEffectHandler, curseEffectHandler,
                _fireEffectHandlerEnemy, doubleEffectHandler, skipTurnEffectHandler, fateInevitabilityHandler, justiceBullEffectHandler,
                _enemyBrothersEffectHandler, scarecrowEffectHandler, wiseMonkEffectHandler, falsePrinceEffectHandler,
                fallenGuardianEffectHandler);

            SkipTurnChecker skipTurnChecker = new SkipTurnChecker(slimeEffectHandler, _enemyHand);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _enemyPlayingZone, _enemyCardAttackZone,
                _discardPile, _drawCardRootEnemy, _playerHand, _historyRoot);
            StartTurnDrawEnemyAICreator startTurnDrawCreator = new StartTurnDrawEnemyAICreator(_interactionActivator, _drawCardRootEnemy, _enemyCountStartDrawCards);
            //StartTurnDrawEnemyAI startTurnDraw = new StartTurnDrawEnemyAI(_interactionActivator, drawCardRoot, 0);
            EnemySkipTurnViewCreator skipTurnViewCreator = new EnemySkipTurnViewCreator(_interactionActivator, _skipTurnLabelEnemyAI);
            OnBeforeEndTurnProcessingCreator onBeforeEndTurnProcessingCreator = new OnBeforeEndTurnProcessingCreator(_interactionActivator, _enemyPersonEffectsHandler);

            //HardAIThinkLogic hardAIThinkLogic = new HardAIThinkLogic(_cardRoot,_deck, _confirmableNumbersEnemyAI, gnomeEffectHandler,
            //    _enemyPlayingZone, _enemyHand, fireEffectHandler, _discardPile, _fireRoot);
            VeryHardAIThinkLogic hardAIThinkLogic = new VeryHardAIThinkLogic(_cardRoot, _deck, _confirmableNumbersEnemyAI, gnomeEffectHandler,
                _enemyPlayingZone, _enemyHand, _fireEffectHandlerEnemy, _discardPile, _fireRoot, _playerHand, _playerEffectKeeper, _confirmableNumbersPlayer,
                _enemyBrothersEffectHandler, _enemyTable, _playerTable, _skipTurnEffectHandlerPlayer, _fateInevitabilityHandlerPlayer, _fireEffectHandlerPlayer,
                wiseMonkEffectHandler, scarecrowEffectHandler);

            EnemyDragAndDropImitationCreator enemyDragAndDropImitationCreator = new EnemyDragAndDropImitationCreator(cardDragAndDropImitationActions,
                _enemyDragAndDropImitationData, _interactionActivator, skipTurnChecker, _enemyTurnDrawnCards, _enemyHand,
                hardAIThinkLogic, gnomeEffectHandler);

            _enemyHand.Init(_seatPool, _enemyRechangeFeatureRuleController, _enemyTurnDrawnCards, curseEffectHandler);

            //LookCardMenuEnemyAI lookCardMenuEnemyAI = new LookCardMenuEnemyAI(_informationLabel);
            LookCardMenuEnemyAI lookCardMenuEnemyAI = new LookCardMenuEnemyAI();

            return new EnemyAI(_interactionActivator, enemyDragAndDropImitationCreator, _enemyPlayingZone,
                _enemyTower, _drawCardRootEnemy, _enemyDiscoverImitation, startTurnDrawCreator, _bus, _enemyHand, _enemyAttackMenu,
                _enemyChoiceMenu, _enemyChoiceMenuImitation, _enemyPersonEffectsHandler, lookCardMenuEnemyAI,
                onBeforeEndTurnProcessingCreator, skipTurnViewCreator, _confirmableNumbersEnemyAI, _lastSelectedNumbersWatcherEnemyAI, _enemyEffectKeeper, _gameFieldToken);
        }

        public CardLocationViewRoot CreateCardLocationViewRoot()
        {
            return new CardLocationViewRoot(_cardRoot, _deck, _playerHand, _enemyHand, _discardPile, _fireRoot, _playerTable, _enemyTable);
        }

        public CardTransitManager CreateCardTransitManager()
        {
            return new CardTransitManager(_playerHand, _enemyHand, _playerTower, _enemyTower, _deck, _discardPile, _fireRoot, _playerFirePool, _enemyFirePool);
        }

        public BrothersEffectHandlerRoot CreateBrothersEffectHandlerRoot()
        {
            return new BrothersEffectHandlerRoot(_playerBrothersEffectHandler, _enemyBrothersEffectHandler);
        }

        public PersonEffectsHandlerRoot CreatePersonEffectsHandlerRoot()
        {
            return new PersonEffectsHandlerRoot(_enemyPersonEffectsHandler, _playerPersonEffectsHandler);
        }

        public GameFieldInputRoot GetInputRoot()
        {
            return _inputRoot;
        }

        public LoseActionsRoot CreateLoseActionsRoot()
        {
            return new LoseActionsRoot(_playerLoseActions, _enemyLoseActions);
        }

        private void InitPlayersData()
        {
            _playerTurnDrawnCards = new TurnDrawnCards();

            //_playerHand.Init(seatPool, _playerRechangeFeatureRuleController, _playerTurnDrawnCards);
            _playerTable.Init();
            _playerPlayingZone.Init(_playerTable);
            _playerTower.Init(_confirmableNumbersEnemyAI, _cardRoot, _fightToken);
            _playerDiscover.Init(_fightToken);
            _startPlayerTurnLabel.Init();

            //SelectNumbersList attackedNumbers = new SelectNumbersList();
            //SelectNumbersList choicedNumbers = new SelectNumbersList();
            //SelectNumbersList cursedNumbers = new SelectNumbersList();

            //_playerLoseActions = new LoseActions(_playerTower ,_playerTower, _playerHand, _bus);
            AttackResultHandlerPlayer attackResultHandlerPlayer = new AttackResultHandlerPlayer(_discardPile, _enemyLoseActions,
                _playerCardAttackZone, _attackResultHandlerPlayerData);
            ChoiceResultHandlerPlayer choiceResultHandlerPlayer = new ChoiceResultHandlerPlayer(_informationLabel, _informationLabelDataPlayerChoice);

            _playerAttackMenu.Init(_enemyTower, attackResultHandlerPlayer, _cardNumbers, _attackedNumbersPlayer,
                _confirmableNumbersPlayer, _inputRoot, _lastSelectedNumbersWatcherPlayer, _UIHelperDescription,
                _fightToken);
            _playerChoiceMenu.Init(_enemyTower, choiceResultHandlerPlayer, _cardNumbers, _choicedNumbersPlayer,
                _confirmableNumbersPlayer, _inputRoot, _lastSelectedNumbersWatcherPlayer, _UIHelperDescription,
                _fightToken);
            _playerChoiceMenuImitation.Init(_enemyTower, choiceResultHandlerPlayer, _cardNumbers, _choicedNumbersPlayer,
                _confirmableNumbersPlayer, _lastSelectedNumbersWatcherPlayer, _fightToken);

            _playerCardAttackZone.Init(_playerAttackMenu, _enemyTower, _bus, _historyRoot);
            //_playerCardAttackZone.Init(_playerChoiceMenu, _enemyTower);

            _skipTurnEffectHandlerPlayer = new SkipTurnEffectHandler();
            _fateInevitabilityHandlerPlayer = new FateInevitabilityHandler(_playerLoseActions, _playerAttackMenu);
            //_scarecrowEffectHandlerPlayer = new ScarecrowEffectHandler(_discardManager);
            //_wiseMonkEffectHandlerPlayer = new WiseMonkEffectHandler();

            CreateDrawCardRootPlayer();
        }

        private void CreateDrawCardRootPlayer()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _playerTurnDrawnCards, _playerSimpleDrawCardAnimationData);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerFireDrawCardAnimationData, _playerFirePool, _playerHand);
            DrawCardAnimationManager drawCardAnimationManager = new DrawCardAnimationManager(simpleDrawCardAnimation, fireDrawCardAnimation);
            _drawCardRootPlayer = new DrawCardRoot(drawCardAnimationManager, _deck);

            _fireEffectHandlerPlayer = new FireEffectHandler(drawCardAnimationManager);
        }

        private void InitEnemyData()
        {
            _enemyTurnDrawnCards = new TurnDrawnCards();

            //_enemyHand.Init(seatPool, _enemyRechangeFeatureRuleController, _enemyTurnDrawnCards);
            _enemyTable.Init();
            _enemyPlayingZone.Init(_enemyTable);
            _enemyTower.Init(_confirmableNumbersViewRoot, _confirmableNumbersPlayer, _cardRoot, _fightToken);

            _enemyDiscoverImitation.Init(_fightToken);

            //SelectNumbersList attackedNumbers = new SelectNumbersList();
            //SelectNumbersList choicedNumbers = new SelectNumbersList();
            //SelectNumbersList cursedNumbers = new SelectNumbersList();

            //TestBotLogic_ChoiceNumbers_TEST4(choicedNumbers);
            //_enemyLoseActions = new LoseActions(_enemyTower , _enemyTower, _playerHand, _bus); 
            AttackResultHandlerEnemyAI attackResultHandlerEnemyAI = new AttackResultHandlerEnemyAI(_discardPile, _playerLoseActions,
                _enemyCardAttackZone, _attackResultHandlerEnemyAIData, _informationLabel, _informationLabelDataEnemyAIAttack);
            ChoiceResultHandlerEnemyAI choiceResultHandlerEnemyAI = new ChoiceResultHandlerEnemyAI(_informationLabel, _informationLabelDataEnemyAIChoice);

            _enemyAttackMenu.Init(_playerTower, attackResultHandlerEnemyAI, _cardNumbers, _attackedNumbersEnemy,
                _confirmableNumbersEnemyAI, _lastSelectedNumbersWatcherEnemyAI, _fightToken);
            _enemyChoiceMenu.Init(_playerTower, choiceResultHandlerEnemyAI, _cardNumbers, _choicedNumbersEnemy,
                _confirmableNumbersEnemyAI, _lastSelectedNumbersWatcherEnemyAI, _fightToken);
            _enemyChoiceMenuImitation.Init(_playerTower, choiceResultHandlerEnemyAI, _cardNumbers, _choicedNumbersEnemy,
                _confirmableNumbersEnemyAI, _lastSelectedNumbersWatcherEnemyAI, _fightToken);

            _enemyCardAttackZone.Init(_enemyAttackMenu, _playerTower, _bus, _historyRoot);

            CreateDrawCardRootEnemy();
        }

        private void CreateDrawCardRootEnemy()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _enemyTurnDrawnCards, _enemyAISimpleDrawCardAnimationData);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyAIFireDrawCardAnimationData, _enemyFirePool, _enemyHand);
            DrawCardAnimationManager drawCardAnimationManager = new DrawCardAnimationManager(simpleDrawCardAnimation, fireDrawCardAnimation);
            _drawCardRootEnemy = new DrawCardRoot(drawCardAnimationManager, _deck);

            _fireEffectHandlerEnemy = new FireEffectHandler(drawCardAnimationManager);
        }

        private void DefineFire()
        {
            PyromancersManuscriptFireAction playerPyromancersManuscriptFireAction = new PyromancersManuscriptFireAction(_cardRoot,
                SideType.Front, _fireContainer.GetTransform());
            ExtraFireSeatActionRoot playerExtraFireSeatActionRoot = new ExtraFireSeatActionRoot(playerPyromancersManuscriptFireAction);

            PyromancersManuscriptFireAction enemyPyromancersManuscriptFireAction = new PyromancersManuscriptFireAction(_cardRoot,
                SideType.Back, _fireContainer.GetTransform());
            ExtraFireSeatActionRoot enemyExtraFireSeatActionRoot = new ExtraFireSeatActionRoot(enemyPyromancersManuscriptFireAction);

            _playerFirePool = new FirePoolPlayer(_fireContainer.GetTransform(), playerExtraFireSeatActionRoot,
                _historyRoot);
            _enemyFirePool = new FirePoolEnemy(_fireContainer.GetTransform(), enemyExtraFireSeatActionRoot,
                _historyRoot);
            _fireRoot = new FireRoot(_playerFirePool, _enemyFirePool);
        }

        //private void InitCommonData()
        //{
        //    _fireRoot = new FireRoot(_playerFirePool, _enemyFirePool);
        //}

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(PersonCreator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                //DefineStartPlayerTurnLabel(),
                //DefineTableActivator(),
                DefineTowerActivator(),
            };

            return list;
        }

        //[ContextMenu(nameof(DefineStartPlayerTurnLabel))]
        //private ComponentAttachInfo DefineStartPlayerTurnLabel()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _startPlayerTurnLabel, ComponentLocationTypes.InScene);
        //}

        //[ContextMenu(nameof(DefineTableActivator))]
        //private ComponentAttachInfo DefineTableActivator()
        //{
        //    return AutomaticFillComponents.DefineComponent(this, ref _tableActivator, ComponentLocationTypes.InScene);
        //}

        [ContextMenu(nameof(DefineTowerActivator))]
        private ComponentAttachInfo DefineTowerActivator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _towerActivator, ComponentLocationTypes.InScene);
        }

        #endregion

        #region TESTS

        private void TestBotLogic_ChoiceNumbers_TEST5(SelectNumbersList choicedNumbers)
        {
            choicedNumbers.Add(2, NumberAnimationType.Choice);
            choicedNumbers.Add(4, NumberAnimationType.Choice);
        }

        private void TestBotLogic_ChoiceNumbers_TEST4(SelectNumbersList choicedNumbers)
        {
            //choicedNumbers.Add(1, NumberAnimationType.Choice);
            //choicedNumbers.Add(2, NumberAnimationType.Choice);
            //choicedNumbers.Add(3, NumberAnimationType.Choice);
            //choicedNumbers.Add(4, NumberAnimationType.Choice);
            choicedNumbers.Add(5, NumberAnimationType.Choice);
            choicedNumbers.Add(6, NumberAnimationType.Choice);
            choicedNumbers.Add(7, NumberAnimationType.Choice);
            choicedNumbers.Add(8, NumberAnimationType.Choice);
            choicedNumbers.Add(9, NumberAnimationType.Choice);
            choicedNumbers.Add(10, NumberAnimationType.Choice);
            choicedNumbers.Add(11, NumberAnimationType.Choice);
            choicedNumbers.Add(12, NumberAnimationType.Choice);
            choicedNumbers.Add(13, NumberAnimationType.Choice);
            choicedNumbers.Add(14, NumberAnimationType.Choice);
            choicedNumbers.Add(15, NumberAnimationType.Choice);
            choicedNumbers.Add(16, NumberAnimationType.Choice);
            choicedNumbers.Add(17, NumberAnimationType.Choice);
            choicedNumbers.Add(18, NumberAnimationType.Choice);
            choicedNumbers.Add(19, NumberAnimationType.Choice);
            choicedNumbers.Add(20, NumberAnimationType.Choice);
            choicedNumbers.Add(21, NumberAnimationType.Choice);
            choicedNumbers.Add(22, NumberAnimationType.Choice);
            choicedNumbers.Add(23, NumberAnimationType.Choice);
            choicedNumbers.Add(24, NumberAnimationType.Choice);
            choicedNumbers.Add(25, NumberAnimationType.Choice);
            choicedNumbers.Add(26, NumberAnimationType.Choice);
            choicedNumbers.Add(27, NumberAnimationType.Choice);
            choicedNumbers.Add(28, NumberAnimationType.Choice);
            choicedNumbers.Add(29, NumberAnimationType.Choice);
            choicedNumbers.Add(30, NumberAnimationType.Choice);
            choicedNumbers.Add(31, NumberAnimationType.Choice);
            choicedNumbers.Add(32, NumberAnimationType.Choice);
            choicedNumbers.Add(33, NumberAnimationType.Choice);
            choicedNumbers.Add(34, NumberAnimationType.Choice);
            choicedNumbers.Add(35, NumberAnimationType.Choice);
            choicedNumbers.Add(36, NumberAnimationType.Choice);
            choicedNumbers.Add(37, NumberAnimationType.Choice);
            choicedNumbers.Add(38, NumberAnimationType.Choice);
            choicedNumbers.Add(39, NumberAnimationType.Choice);
            choicedNumbers.Add(40, NumberAnimationType.Choice);
            choicedNumbers.Add(41, NumberAnimationType.Choice);
            choicedNumbers.Add(42, NumberAnimationType.Choice);
            choicedNumbers.Add(43, NumberAnimationType.Choice);
            choicedNumbers.Add(44, NumberAnimationType.Choice);
            choicedNumbers.Add(45, NumberAnimationType.Choice);
            //choicedNumbers.Add(46, NumberAnimationType.Choice);
            choicedNumbers.Add(47, NumberAnimationType.Choice);
            //choicedNumbers.Add(48, NumberAnimationType.Choice);
            choicedNumbers.Add(49, NumberAnimationType.Choice);
            choicedNumbers.Add(50, NumberAnimationType.Choice);
        }

        private void TestBotLogic_ChoiceNumbers_TEST3(SelectNumbersList choicedNumbers)
        {
            choicedNumbers.Add(1, NumberAnimationType.Choice);
            choicedNumbers.Add(2, NumberAnimationType.Choice);
            choicedNumbers.Add(3, NumberAnimationType.Choice);
            choicedNumbers.Add(4, NumberAnimationType.Choice);
            choicedNumbers.Add(5, NumberAnimationType.Choice);
            choicedNumbers.Add(6, NumberAnimationType.Choice);
            choicedNumbers.Add(7, NumberAnimationType.Choice);
            choicedNumbers.Add(10, NumberAnimationType.Choice);
            choicedNumbers.Add(11, NumberAnimationType.Choice);
            choicedNumbers.Add(12, NumberAnimationType.Choice);
            choicedNumbers.Add(13, NumberAnimationType.Choice);
            choicedNumbers.Add(14, NumberAnimationType.Choice);
            choicedNumbers.Add(15, NumberAnimationType.Choice);
            choicedNumbers.Add(16, NumberAnimationType.Choice);
            choicedNumbers.Add(17, NumberAnimationType.Choice);
            choicedNumbers.Add(18, NumberAnimationType.Choice);
            choicedNumbers.Add(19, NumberAnimationType.Choice);
            choicedNumbers.Add(20, NumberAnimationType.Choice);
            choicedNumbers.Add(21, NumberAnimationType.Choice);
            choicedNumbers.Add(22, NumberAnimationType.Choice);
            choicedNumbers.Add(23, NumberAnimationType.Choice);
            choicedNumbers.Add(24, NumberAnimationType.Choice);
            choicedNumbers.Add(25, NumberAnimationType.Choice);
            choicedNumbers.Add(26, NumberAnimationType.Choice);
            choicedNumbers.Add(27, NumberAnimationType.Choice);
            choicedNumbers.Add(28, NumberAnimationType.Choice);
            choicedNumbers.Add(29, NumberAnimationType.Choice);
            choicedNumbers.Add(30, NumberAnimationType.Choice);
            choicedNumbers.Add(31, NumberAnimationType.Choice);
            choicedNumbers.Add(32, NumberAnimationType.Choice);
            choicedNumbers.Add(33, NumberAnimationType.Choice);
            choicedNumbers.Add(34, NumberAnimationType.Choice);
            choicedNumbers.Add(39, NumberAnimationType.Choice);
            choicedNumbers.Add(40, NumberAnimationType.Choice);
            choicedNumbers.Add(41, NumberAnimationType.Choice);
            choicedNumbers.Add(42, NumberAnimationType.Choice);
            choicedNumbers.Add(43, NumberAnimationType.Choice);
            choicedNumbers.Add(44, NumberAnimationType.Choice);
            choicedNumbers.Add(45, NumberAnimationType.Choice);
            choicedNumbers.Add(47, NumberAnimationType.Choice);
            choicedNumbers.Add(48, NumberAnimationType.Choice);
            choicedNumbers.Add(49, NumberAnimationType.Choice);
        }

        private void TestBotLogic_ChoiceNumbers_TEST2(SelectNumbersList choicedNumbers)
        {
            choicedNumbers.Add(1, NumberAnimationType.Choice);
            choicedNumbers.Add(2, NumberAnimationType.Choice);
            choicedNumbers.Add(3, NumberAnimationType.Choice);
            choicedNumbers.Add(4, NumberAnimationType.Choice);
            choicedNumbers.Add(5, NumberAnimationType.Choice);
            choicedNumbers.Add(6, NumberAnimationType.Choice);
            choicedNumbers.Add(7, NumberAnimationType.Choice);
            choicedNumbers.Add(46, NumberAnimationType.Choice);
            choicedNumbers.Add(47, NumberAnimationType.Choice);
            choicedNumbers.Add(48, NumberAnimationType.Choice);
            choicedNumbers.Add(49, NumberAnimationType.Choice);
        }

        private void TestBotLogic_ChoiceNumbers_TEST1(SelectNumbersList choicedNumbers)
        {
            choicedNumbers.Add(1, NumberAnimationType.Choice);
            choicedNumbers.Add(3, NumberAnimationType.Choice);
            choicedNumbers.Add(5, NumberAnimationType.Choice);
            choicedNumbers.Add(6, NumberAnimationType.Choice);
            choicedNumbers.Add(7, NumberAnimationType.Choice);
            choicedNumbers.Add(10, NumberAnimationType.Choice);
            choicedNumbers.Add(11, NumberAnimationType.Choice);
            choicedNumbers.Add(13, NumberAnimationType.Choice);
            choicedNumbers.Add(14, NumberAnimationType.Choice);
            choicedNumbers.Add(17, NumberAnimationType.Choice);
            choicedNumbers.Add(19, NumberAnimationType.Choice);
            choicedNumbers.Add(21, NumberAnimationType.Choice);
            choicedNumbers.Add(22, NumberAnimationType.Choice);
            choicedNumbers.Add(25, NumberAnimationType.Choice);
            choicedNumbers.Add(28, NumberAnimationType.Choice);
            choicedNumbers.Add(31, NumberAnimationType.Choice);
            choicedNumbers.Add(34, NumberAnimationType.Choice);
            choicedNumbers.Add(39, NumberAnimationType.Choice);
            choicedNumbers.Add(41, NumberAnimationType.Choice);
            choicedNumbers.Add(43, NumberAnimationType.Choice);
            choicedNumbers.Add(45, NumberAnimationType.Choice);
            choicedNumbers.Add(47, NumberAnimationType.Choice);
            choicedNumbers.Add(48, NumberAnimationType.Choice);
            choicedNumbers.Add(49, NumberAnimationType.Choice);
        }
        #endregion
    }
}