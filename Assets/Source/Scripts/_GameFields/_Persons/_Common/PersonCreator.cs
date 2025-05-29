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
using GameFields.Persons.EffectCounters;

namespace GameFields.Persons.Common
{
    public class PersonCreator : MonoBehaviour, IAutomaticFillComponents
    {
        private const int CountNumbers = 50;

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
        //[SerializeField] private float _fireDrawCardDelay = 2f;
        //[SerializeField] private InvertCardAnimationData _fireAnimationInvertData;

        private SignalBus _bus;
        private Deck _deck;
        private EndTurnButton _endTurnButton;

        private InteractionActivator _interactionActivator;
        private InformationLabel _informationLabel;
        private DiscardPile _discardPile;
        private FireRoot _fireRoot;

        [Inject]
        public void Construct(CardPlayingZonePlayer playerPlayingZone, HandPlayer playerHand, TablePlayer playerTable, TowerPlayer playerTower,
            DiscoverPlayer playerDiscover, AttackMenuPlayer playerAttackMenu, CardPlayingZoneAI enemyPlayingZone, HandAI enemyHand,
            TableAI enemyTable, TowerAI enemyTower, DiscoverAI enemyDiscoverImitation, AttackMenuEnemyAI enemyAttackMenu,
            CardAttackZonePlayer playerCardAttackZone, CardAttackZoneEnemyAI enemyCardAttackZone, ChoiceMenuPlayer playerChoiceMenu,
            ChoiceMenuEnemyAI enemyChoiceMenu, DiscardPile discardPile, ChoiceMenuImitationPlayer choiceMenuImitationPlayer,
            ChoiceMenuImitationEnemyAI choiceMenuImitationEnemyAI)
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

        public void Init(SignalBus bus, Deck deck, EndTurnButton endTurnButton, SeatPool seatPool
            , CardDragAndDropHandler cardDragAndDropHandler, CardDragAndDropLightController cardDragAndDropLightController,
            InformationLabel informationLabel, ForgingZone forgingZone)
        {
            _bus = bus;
            _deck = deck;
            _endTurnButton = endTurnButton;
            _informationLabel = informationLabel;

            _playerFirePool = new FirePool();
            _enemyFirePool = new FirePool();
            _fireRoot = new FireRoot(_playerFirePool, _enemyFirePool);

            _playerRechangeFeatureRuleController = new RechangeFeatureRuleController();
            _enemyRechangeFeatureRuleController = new RechangeFeatureRuleController();

            _interactionActivator = new InteractionActivator(cardDragAndDropHandler, _towerActivator, _tableActivator, endTurnButton,
                cardDragAndDropLightController, forgingZone);

            InitPlayersData(seatPool);
            InitEnemyData(seatPool);
            //InitCommonData();

        }

        public Player CreatePlayer()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _playerSimpleDrawCardAnimationData);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerFireDrawCardAnimationData, _playerFirePool);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _playerSimpleDrawCardAnimationData), _deck);
            TurnProcessing turnProcessing = new TurnProcessing(_interactionActivator, _playerHand);
            StartTurnDrawPlayer startTurnDraw = new StartTurnDrawPlayer(_interactionActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, _playerCountStartDrawCards);
            StartPlayerTurnView startPlayerTurnView = new StartPlayerTurnView(_interactionActivator, _startPlayerTurnLabel);
            EndTurnProcessing endTurnProcessing = new EndTurnProcessing(_endTurnButton, _interactionActivator);

            List<ICardFeatureRechangable> cardFeatureRechangables = new List<ICardFeatureRechangable>()
            {
                _playerTower,
                _playerHand
            };
            GnomeEffectCounter gnomeEffectCounter = new GnomeEffectCounter(_playerRechangeFeatureRuleController, cardFeatureRechangables);
            PersonEffectsCounter personEffectsCounter = new PersonEffectsCounter(gnomeEffectCounter);

            return new Player(_interactionActivator, _playerHand, _playerPlayingZone, _playerTower, _playerDiscover,
                drawCardRoot, startTurnDraw, turnProcessing, _bus, startPlayerTurnView, _playerAttackMenu, endTurnProcessing,
                _playerChoiceMenu, _playerChoiceMenuImitation, personEffectsCounter);
        }

        public EnemyAI CreateEnemyAI()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _enemyAISimpleDrawCardAnimationData);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyAIFireDrawCardAnimationData, _enemyFirePool);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _enemyAISimpleDrawCardAnimationData), _deck);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _enemyPlayingZone, _enemyCardAttackZone);
            //CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _playerTower, _enemyCardAttackZone);
            StartTurnDrawEnemyAI startTurnDraw = new StartTurnDrawEnemyAI(_interactionActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, _enemyCountStartDrawCards);
            EnemyDragAndDropImitation enemyDragAndDropImitation = new EnemyDragAndDropImitation(cardDragAndDropImitationActions,
                _enemyDragAndDropImitationData, _interactionActivator, _enemyHand);

            List<ICardFeatureRechangable> cardFeatureRechangables = new List<ICardFeatureRechangable>()
            {
                _enemyTower,
                _enemyHand
            };
            GnomeEffectCounter gnomeEffectCounter = new GnomeEffectCounter(_enemyRechangeFeatureRuleController, cardFeatureRechangables);
            PersonEffectsCounter personEffectsCounter = new PersonEffectsCounter(gnomeEffectCounter);

            return new EnemyAI(_interactionActivator, enemyDragAndDropImitation, _enemyPlayingZone,
                _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw, _bus, _enemyHand, _playerAttackMenu,
                _enemyChoiceMenu, _enemyChoiceMenuImitation, personEffectsCounter);
        }

        public CardLocationViewRoot CreateCardLocationViewRoot(ICardWatcher cardWatcher)
        {
            return new CardLocationViewRoot(cardWatcher, _deck, _playerHand, _enemyHand, _discardPile, _fireRoot);
        }

        public CardTransitManager CreateCardTransitManager()
        {
            return new CardTransitManager(_playerHand, _enemyHand, _playerTower, _enemyTower, _deck, _discardPile);
        }

        private void InitPlayersData(SeatPool seatPool)
        {
            _playerHand.Init(seatPool, _playerRechangeFeatureRuleController);
            _playerTable.Init();
            _playerPlayingZone.Init(_playerTable);
            _playerTower.Init();
            _playerDiscover.Init();
            _startPlayerTurnLabel.Init();

            SelectNumbersList attackedNumbers = new SelectNumbersList();
            SelectNumbersList choicedNumbers = new SelectNumbersList();

            ConfirmableNumbers confirmableNumbersPlayer = new ConfirmableNumbers(attackedNumbers, choicedNumbers);

            AttackResultHandlerPlayer attackResultHandlerPlayer = new AttackResultHandlerPlayer(_discardPile, _bus,
                _enemyTower, _playerCardAttackZone, _attackResultHandlerPlayerData);
            ChoiceResultHandlerPlayer choiceResultHandlerPlayer = new ChoiceResultHandlerPlayer(_informationLabel, _informationLabelDataPlayerChoice);

            _playerAttackMenu.Init(_enemyTower, attackResultHandlerPlayer, CountNumbers, attackedNumbers, confirmableNumbersPlayer);
            _playerChoiceMenu.Init(_enemyTower, choiceResultHandlerPlayer, CountNumbers, choicedNumbers, confirmableNumbersPlayer);
            _playerChoiceMenuImitation.Init(_enemyTower, choiceResultHandlerPlayer, CountNumbers, choicedNumbers, confirmableNumbersPlayer);

            _playerCardAttackZone.Init(_playerAttackMenu, _enemyTower, _bus);
            //_playerCardAttackZone.Init(_playerChoiceMenu, _enemyTower);
        }

        private void InitEnemyData(SeatPool seatPool)
        {
            _enemyHand.Init(seatPool, _enemyRechangeFeatureRuleController);
            _enemyTable.Init();
            _enemyPlayingZone.Init(_enemyTable);
            _enemyTower.Init();
            _enemyDiscoverImitation.Init();

            SelectNumbersList attackedNumbers = new SelectNumbersList();
            SelectNumbersList choicedNumbers = new SelectNumbersList();

            ConfirmableNumbers confirmableNumbersEnemyAI = new ConfirmableNumbers(attackedNumbers, choicedNumbers);

            //TestBotLogic_ChoiceNumbers_TEST4(choicedNumbers);

            AttackResultHandlerEnemyAI attackResultHandlerEnemyAI = new AttackResultHandlerEnemyAI(_discardPile, _bus, _playerTower,
                _enemyCardAttackZone, _attackResultHandlerEnemyAIData, _informationLabel, _informationLabelDataEnemyAIAttack);
            ChoiceResultHandlerEnemyAI choiceResultHandlerEnemyAI = new ChoiceResultHandlerEnemyAI(_informationLabel, _informationLabelDataEnemyAIChoice);

            _enemyAttackMenu.Init(_playerTower, attackResultHandlerEnemyAI, CountNumbers, attackedNumbers, confirmableNumbersEnemyAI);
            _enemyChoiceMenu.Init(_playerTower, choiceResultHandlerEnemyAI, CountNumbers, choicedNumbers, confirmableNumbersEnemyAI);
            _enemyChoiceMenuImitation.Init(_playerTower, choiceResultHandlerEnemyAI, CountNumbers, choicedNumbers, confirmableNumbersEnemyAI);

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