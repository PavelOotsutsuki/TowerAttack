using System.Collections.Generic;
using GameFields.CommonAnimations;
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
using GameFields.Persons.SelectMenues.Commons;
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

namespace GameFields.Persons.Commons
{
    public class PersonCreator : MonoBehaviour, IAutomaticFillComponents
    {
        private readonly int[] _cardNumbers = GameSettings.DefaultCardNumbers;

        [Header("Player Fields:")]

        private CardPlayingZone _playerPlayingZone;
        private HandPlayer _playerHand;
        private Table _playerTable;
        private Tower _playerTower;
        private DiscoverPlayer _playerDiscover;
        private ChoiceMenuPlayer _playerChoiceMenu;
        private ChoiceMenuImitationPlayer _playerChoiceMenuImitation;
        private AttackMenuPlayer _playerAttackMenu;
        private CardAttackZonePlayer _playerCardAttackZone;
        private FirePool _playerFirePool;
        private RechangeFeatureRuleController _playerRechangeFeatureRuleController;
        private TurnDrawnCards _playerTurnDrawnCards;

        private LookCardMenuPlayer _playerLookCardMenu;

        private ForgingZone _forgingZone;
        private HandTransferZone _handTransferZone;

        private SelectNumbersList _attackedNumbersPlayer = new SelectNumbersList();
        private SelectNumbersList _choicedNumbersPlayer = new SelectNumbersList();
        private SelectNumbersList _cursedNumbersPlayer = new SelectNumbersList();
        private ConfirmableNumbers _confirmableNumbersPlayer;

        private LoseActions _playerLoseActions;

        [SerializeField] private StartPlayerTurnLabel _startPlayerTurnLabel; 
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
        private Tower _enemyTower;
        private DiscoverAI _enemyDiscoverImitation;
        private ChoiceMenuEnemyAI _enemyChoiceMenu;
        private ChoiceMenuImitationEnemyAI _enemyChoiceMenuImitation;
        private AttackMenuEnemyAI _enemyAttackMenu;
        private CardAttackZoneEnemyAI _enemyCardAttackZone;
        private FirePool _enemyFirePool;
        private RechangeFeatureRuleController _enemyRechangeFeatureRuleController;
        private TurnDrawnCards _enemyTurnDrawnCards;

        private SelectNumbersList _attackedNumbersEnemy = new SelectNumbersList();
        private SelectNumbersList _choicedNumbersEnemy = new SelectNumbersList();
        private SelectNumbersList _cursedNumbersEnemyAI = new SelectNumbersList();
        private ConfirmableNumbers _confirmableNumbersEnemyAI;

        private LoseActions _enemyLoseActions;

        [SerializeField] private int _enemyCountStartDrawCards = 1;
        
        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("GameFieldObjectsActivator:")]

        [SerializeField] private TableActivator _tableActivator;
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

        private InteractionActivator _interactionActivator;
        private InformationLabel _informationLabel;
        private DiscardPile _discardPile;
        private FireRoot _fireRoot;
        private ICardWatcher _cardWatcher;

        [Inject]
        public void Construct(CardPlayingZonePlayer playerPlayingZone, HandPlayer playerHand, TablePlayer playerTable, TowerPlayer playerTower,
            DiscoverPlayer playerDiscover, AttackMenuPlayer playerAttackMenu, CardPlayingZoneAI enemyPlayingZone, HandAI enemyHand,
            TableAI enemyTable, TowerAI enemyTower, DiscoverAI enemyDiscoverImitation, AttackMenuEnemyAI enemyAttackMenu,
            CardAttackZonePlayer playerCardAttackZone, CardAttackZoneEnemyAI enemyCardAttackZone, ChoiceMenuPlayer playerChoiceMenu,
            ChoiceMenuEnemyAI enemyChoiceMenu, DiscardPile discardPile, ChoiceMenuImitationPlayer choiceMenuImitationPlayer,
            ChoiceMenuImitationEnemyAI choiceMenuImitationEnemyAI, ForgingZone forgingZone, HandTransferZone handTransferZone,
            LookCardMenuPlayer lookCardMenuPlayer)
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
        }

        public void Init(SignalBus bus, Deck deck, EndTurnButton endTurnButton, SeatPool seatPool,
            CardDragAndDropHandler cardDragAndDropHandler, CardDragAndDropLightController cardDragAndDropLightController,
            InformationLabel informationLabel, ICardWatcher cardRoot)
        {
            _bus = bus;
            _deck = deck;
            _endTurnButton = endTurnButton;
            _informationLabel = informationLabel;
            _cardWatcher = cardRoot;

            _playerFirePool = new FirePool(_fireContainer.GetTransform());
            _enemyFirePool = new FirePool(_fireContainer.GetTransform());
            _fireRoot = new FireRoot(_playerFirePool, _enemyFirePool);

            _playerRechangeFeatureRuleController = new RechangeFeatureRuleController();
            _enemyRechangeFeatureRuleController = new RechangeFeatureRuleController();

            _interactionActivator = new InteractionActivator(cardDragAndDropHandler, _towerActivator, _tableActivator, endTurnButton,
                cardDragAndDropLightController, _forgingZone, _handTransferZone);

            _seatPool = seatPool;

            _enemyLoseActions = new LoseActions(_enemyTower, _enemyTower, _playerHand, _bus);
            _playerLoseActions = new LoseActions(_playerTower, _playerTower, _playerHand, _bus);


            InitPlayersData();
            InitEnemyData();
            //InitCommonData();

        }

        public Player CreatePlayer()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _playerTurnDrawnCards, _playerSimpleDrawCardAnimationData);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerFireDrawCardAnimationData, _playerFirePool);
            DrawCardAnimationManager drawCardAnimationManager = new DrawCardAnimationManager(simpleDrawCardAnimation, fireDrawCardAnimation);
            DrawCardRoot drawCardRoot = new DrawCardRoot(drawCardAnimationManager, _deck);

            SlimeEffectHandler slimeEffectHandler = new SlimeEffectHandler(_playerHand, _playerTurnDrawnCards);
            List<ICardFeatureRechangable> cardFeatureRechangables = new List<ICardFeatureRechangable>()
            {
                _playerTower,
                _playerHand
            };
            GnomeEffectHandler gnomeEffectHandler = new GnomeEffectHandler(_playerRechangeFeatureRuleController, cardFeatureRechangables);
            CurseEffectHandlerPlayer curseEffectHandler = new CurseEffectHandlerPlayer(_playerTower, _informationLabel, _confirmableNumbersEnemyAI,
                _cursedNumbersEnemyAI, _bus);
            FireEffectHandler fireEffectHandler = new FireEffectHandler(drawCardAnimationManager);
            DoubleEffectHandler doubleEffectHandler = new DoubleEffectHandler();
            SkipTurnEffectHandler skipTurnEffectHandler = new SkipTurnEffectHandler();
            FateInevitabilityHandler fateInevitabilityHandler = new FateInevitabilityHandler(_playerLoseActions, _playerAttackMenu);
            PersonEffectsHandler personEffectsHandler = new PersonEffectsHandler(gnomeEffectHandler, slimeEffectHandler, curseEffectHandler,
                fireEffectHandler, doubleEffectHandler, skipTurnEffectHandler, fateInevitabilityHandler);

            SkipTurnChecker skipTurnChecker = new SkipTurnChecker(slimeEffectHandler, _playerHand);
            TurnProcessing turnProcessing = new TurnProcessing(_interactionActivator, skipTurnChecker);
            StartTurnDrawPlayer startTurnDraw = new StartTurnDrawPlayer(_interactionActivator, drawCardRoot, _playerCountStartDrawCards);

            StartPlayerTurnView startPlayerTurnView = new StartPlayerTurnView(_interactionActivator, _startPlayerTurnLabel);
            EndTurnProcessing endTurnProcessing = new EndTurnProcessing(_endTurnButton, _interactionActivator, personEffectsHandler);

            _forgingZone.Init(_discardPile, _bus, drawCardRoot, gnomeEffectHandler);
            _handTransferZone.Init(_enemyHand, _bus);

            _playerHand.Init(_seatPool, _playerRechangeFeatureRuleController, _playerTurnDrawnCards, curseEffectHandler);

            return new Player(_interactionActivator, _playerHand, _playerPlayingZone, _playerTower, _playerDiscover,
                drawCardRoot, startTurnDraw, turnProcessing, _bus, startPlayerTurnView, _playerAttackMenu, endTurnProcessing,
                _playerChoiceMenu, _playerChoiceMenuImitation, personEffectsHandler, _informationLabel, _playerLookCardMenu,
                _playerLoseActions);
        }

        public EnemyAI CreateEnemyAI()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _enemyTurnDrawnCards, _enemyAISimpleDrawCardAnimationData);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyAIFireDrawCardAnimationData, _enemyFirePool);
            DrawCardAnimationManager drawCardAnimationManager = new DrawCardAnimationManager(simpleDrawCardAnimation, fireDrawCardAnimation);
            DrawCardRoot drawCardRoot = new DrawCardRoot(drawCardAnimationManager, _deck);

            SlimeEffectHandler slimeEffectHandler = new SlimeEffectHandler(_enemyHand, _enemyTurnDrawnCards);
            List<ICardFeatureRechangable> cardFeatureRechangables = new List<ICardFeatureRechangable>()
            {
                _enemyTower,
                _enemyHand
            };
            GnomeEffectHandler gnomeEffectHandler = new GnomeEffectHandler(_enemyRechangeFeatureRuleController, cardFeatureRechangables);
            CurseEffectHandlerEnemyAI curseEffectHandler = new CurseEffectHandlerEnemyAI(_enemyTower, _informationLabel, _confirmableNumbersPlayer,
                _cursedNumbersPlayer, _bus);
            FireEffectHandler fireEffectHandler = new FireEffectHandler(drawCardAnimationManager);
            DoubleEffectHandler doubleEffectHandler = new DoubleEffectHandler();
            SkipTurnEffectHandler skipTurnEffectHandler = new SkipTurnEffectHandler();
            FateInevitabilityHandler fateInevitabilityHandler = new FateInevitabilityHandler(_enemyLoseActions, _enemyAttackMenu);
            PersonEffectsHandler personEffectsHandler = new PersonEffectsHandler(gnomeEffectHandler, slimeEffectHandler, curseEffectHandler,
                fireEffectHandler, doubleEffectHandler, skipTurnEffectHandler, fateInevitabilityHandler);

            SkipTurnChecker skipTurnChecker = new SkipTurnChecker(slimeEffectHandler, _enemyHand);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _enemyPlayingZone, _enemyCardAttackZone,
                _discardPile, drawCardRoot, _playerHand);
            StartTurnDrawEnemyAI startTurnDraw = new StartTurnDrawEnemyAI(_interactionActivator, drawCardRoot, _enemyCountStartDrawCards);
            OnBeforeEndTurnProcessing onBeforeEndTurnProcessing = new OnBeforeEndTurnProcessing(_interactionActivator, personEffectsHandler);

            HardAIThinkLogic hardAIThinkLogic = new HardAIThinkLogic(_cardWatcher,_deck, _confirmableNumbersEnemyAI, gnomeEffectHandler,
                _enemyPlayingZone, _enemyHand, fireEffectHandler, _discardPile, _fireRoot);
            EnemyDragAndDropImitation enemyDragAndDropImitation = new EnemyDragAndDropImitation(cardDragAndDropImitationActions,
                _enemyDragAndDropImitationData, _interactionActivator, skipTurnChecker, _enemyTurnDrawnCards, _enemyHand,
                hardAIThinkLogic, gnomeEffectHandler);

            _enemyHand.Init(_seatPool, _enemyRechangeFeatureRuleController, _enemyTurnDrawnCards, curseEffectHandler);

            LookCardMenuEnemyAI lookCardMenuEnemyAI = new LookCardMenuEnemyAI(_informationLabel);

            return new EnemyAI(_interactionActivator, enemyDragAndDropImitation, _enemyPlayingZone,
                _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw, _bus, _enemyHand, _enemyAttackMenu,
                _enemyChoiceMenu, _enemyChoiceMenuImitation, personEffectsHandler, lookCardMenuEnemyAI, _enemyLoseActions,
                onBeforeEndTurnProcessing);
        }

        public CardLocationViewRoot CreateCardLocationViewRoot()
        {
            return new CardLocationViewRoot(_cardWatcher, _deck, _playerHand, _enemyHand, _discardPile, _fireRoot);
        }

        public CardTransitManager CreateCardTransitManager()
        {
            return new CardTransitManager(_playerHand, _enemyHand, _playerTower, _enemyTower, _deck, _discardPile, _fireRoot, _playerFirePool, _enemyFirePool);
        }

        private void InitPlayersData()
        {
            _playerTurnDrawnCards = new TurnDrawnCards();

            //_playerHand.Init(seatPool, _playerRechangeFeatureRuleController, _playerTurnDrawnCards);
            _playerTable.Init();
            _playerPlayingZone.Init(_playerTable);
            _playerTower.Init();
            _playerDiscover.Init();
            _startPlayerTurnLabel.Init();

            //SelectNumbersList attackedNumbers = new SelectNumbersList();
            //SelectNumbersList choicedNumbers = new SelectNumbersList();
            //SelectNumbersList cursedNumbers = new SelectNumbersList();

            _confirmableNumbersPlayer = new ConfirmableNumbers(_attackedNumbersPlayer, _choicedNumbersPlayer, _cursedNumbersPlayer);

            //_playerLoseActions = new LoseActions(_playerTower ,_playerTower, _playerHand, _bus);
            AttackResultHandlerPlayer attackResultHandlerPlayer = new AttackResultHandlerPlayer(_discardPile, _enemyLoseActions,
                _playerCardAttackZone, _attackResultHandlerPlayerData);
            ChoiceResultHandlerPlayer choiceResultHandlerPlayer = new ChoiceResultHandlerPlayer(_informationLabel, _informationLabelDataPlayerChoice);

            _playerAttackMenu.Init(_enemyTower, attackResultHandlerPlayer, _cardNumbers, _attackedNumbersPlayer, _confirmableNumbersPlayer);
            _playerChoiceMenu.Init(_enemyTower, choiceResultHandlerPlayer, _cardNumbers, _choicedNumbersPlayer, _confirmableNumbersPlayer);
            _playerChoiceMenuImitation.Init(_enemyTower, choiceResultHandlerPlayer, _cardNumbers, _choicedNumbersPlayer, _confirmableNumbersPlayer);

            _playerCardAttackZone.Init(_playerAttackMenu, _enemyTower, _bus);
            //_playerCardAttackZone.Init(_playerChoiceMenu, _enemyTower);
        }

        private void InitEnemyData()
        {
            _enemyTurnDrawnCards = new TurnDrawnCards();

            //_enemyHand.Init(seatPool, _enemyRechangeFeatureRuleController, _enemyTurnDrawnCards);
            _enemyTable.Init();
            _enemyPlayingZone.Init(_enemyTable);
            _enemyTower.Init();
            _enemyDiscoverImitation.Init();

            //SelectNumbersList attackedNumbers = new SelectNumbersList();
            //SelectNumbersList choicedNumbers = new SelectNumbersList();
            //SelectNumbersList cursedNumbers = new SelectNumbersList();

            _confirmableNumbersEnemyAI = new ConfirmableNumbers(_attackedNumbersEnemy, _choicedNumbersEnemy, _cursedNumbersEnemyAI);

            //TestBotLogic_ChoiceNumbers_TEST4(choicedNumbers);
            //_enemyLoseActions = new LoseActions(_enemyTower , _enemyTower, _playerHand, _bus); 
            AttackResultHandlerEnemyAI attackResultHandlerEnemyAI = new AttackResultHandlerEnemyAI(_discardPile, _playerLoseActions,
                _enemyCardAttackZone, _attackResultHandlerEnemyAIData, _informationLabel, _informationLabelDataEnemyAIAttack);
            ChoiceResultHandlerEnemyAI choiceResultHandlerEnemyAI = new ChoiceResultHandlerEnemyAI(_informationLabel, _informationLabelDataEnemyAIChoice);

            _enemyAttackMenu.Init(_playerTower, attackResultHandlerEnemyAI, _cardNumbers, _attackedNumbersEnemy, _confirmableNumbersEnemyAI);
            _enemyChoiceMenu.Init(_playerTower, choiceResultHandlerEnemyAI, _cardNumbers, _choicedNumbersEnemy, _confirmableNumbersEnemyAI);
            _enemyChoiceMenuImitation.Init(_playerTower, choiceResultHandlerEnemyAI, _cardNumbers, _choicedNumbersEnemy, _confirmableNumbersEnemyAI);

            _enemyCardAttackZone.Init(_enemyAttackMenu, _playerTower, _bus);
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
                DefineStartPlayerTurnLabel(),
                DefineTableActivator(),
                DefineTowerActivator(),
            };

            return list;
        }

        [ContextMenu(nameof(DefineStartPlayerTurnLabel))]
        private ComponentAttachInfo DefineStartPlayerTurnLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _startPlayerTurnLabel, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineTableActivator))]
        private ComponentAttachInfo DefineTableActivator()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _tableActivator, ComponentLocationTypes.InScene);
        }

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