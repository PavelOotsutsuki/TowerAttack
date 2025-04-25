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

namespace GameFields.Persons
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
        private AttackMenuPlayer _playerAttackMenu;
        private CardAttackZonePlayer _playerCardAttackZone;

        [SerializeField] private StartPlayerTurnLabel _startPlayerTurnLabel; 
        [SerializeField] private int _playerCountStartDrawCards = 1;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("EnemyAI Fields:")]

        [SerializeField] private EnemyDragAndDropImitationData _enemyDragAndDropImitationData;

        private CardPlayingZone _enemyPlayingZone;
        private HandAI _enemyHand;
        private Table _enemyTable;
        private Tower _enemyTower;
        private DiscoverAI _enemyDiscoverImitation;
        private ChoiceMenuImitation _enemyChoiceMenu;
        private AttackMenuImitation _enemyAttackMenu;
        private CardAttackZoneEnemyAI _enemyCardAttackZone;

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

        [Inject]
        public void Construct(CardPlayingZonePlayer playerPlayingZone, HandPlayer playerHand, TablePlayer playerTable, TowerPlayer playerTower,
            DiscoverPlayer playerDiscover, AttackMenuPlayer playerAttackMenu, CardPlayingZoneAI enemyPlayingZone, HandAI enemyHand,
            TableAI enemyTable, TowerAI enemyTower, DiscoverAI enemyDiscoverImitation, AttackMenuImitation enemyAttackMenu,
            CardAttackZonePlayer playerCardAttackZone, CardAttackZoneEnemyAI enemyCardAttackZone, ChoiceMenuPlayer playerChoiceMenu,
            ChoiceMenuImitation enemyChoiceMenu, InformationLabel informationLabel)
        {
            _playerPlayingZone = playerPlayingZone;
            _playerHand = playerHand;
            _playerTable = playerTable;
            _playerTower = playerTower;
            _playerDiscover = playerDiscover;
            _playerChoiceMenu = playerChoiceMenu;
            _playerAttackMenu = playerAttackMenu;
            _playerCardAttackZone = playerCardAttackZone;

            _enemyPlayingZone = enemyPlayingZone;
            _enemyHand = enemyHand;
            _enemyTable = enemyTable;
            _enemyTower = enemyTower;
            _enemyDiscoverImitation = enemyDiscoverImitation;
            _enemyChoiceMenu = enemyChoiceMenu;
            _enemyAttackMenu = enemyAttackMenu;
            _enemyCardAttackZone = enemyCardAttackZone;

            _informationLabel = informationLabel;
        }

        public void Init(SignalBus bus, Deck deck, EndTurnButton endTurnButton, SeatPool seatPool
            , CardDragAndDropHandler cardDragAndDropHandler, LightController cardDragAndDropLightController)
        {
            _bus = bus;
            _deck = deck;
            _endTurnButton = endTurnButton;

            _interactionActivator = new InteractionActivator(cardDragAndDropHandler, _towerActivator, _tableActivator, endTurnButton, cardDragAndDropLightController);

            InitPlayersData(seatPool);
            InitEnemyData(seatPool);
            //InitCommonData();
        }

        public Player CreatePlayer()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _playerSimpleDrawCardAnimationData);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerFireDrawCardAnimationData);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _playerSimpleDrawCardAnimationData), _deck);
            TurnProcessing turnProcessing = new TurnProcessing(_interactionActivator, _playerHand);
            StartTurnDrawPlayer startTurnDraw = new StartTurnDrawPlayer(_interactionActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, _playerCountStartDrawCards);
            StartPlayerTurnView startPlayerTurnView = new StartPlayerTurnView(_interactionActivator, _startPlayerTurnLabel);
            EndTurnProcessing endTurnProcessing = new EndTurnProcessing(_endTurnButton, _interactionActivator);

            return new Player(_interactionActivator, _playerHand, _playerPlayingZone, _playerTower, _playerDiscover,
                drawCardRoot, startTurnDraw, turnProcessing, _bus, startPlayerTurnView, _playerAttackMenu, endTurnProcessing,
                _playerChoiceMenu);
        }

        public EnemyAI CreateEnemyAI()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _enemyAISimpleDrawCardAnimationData);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyAIFireDrawCardAnimationData);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _enemyAISimpleDrawCardAnimationData), _deck);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _enemyPlayingZone, _enemyCardAttackZone);
            //CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _playerTower, _enemyCardAttackZone);
            StartTurnDrawEnemyAI startTurnDraw = new StartTurnDrawEnemyAI(_interactionActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, _enemyCountStartDrawCards);
            EnemyDragAndDropImitation enemyDragAndDropImitation = new EnemyDragAndDropImitation(cardDragAndDropImitationActions,
                _enemyDragAndDropImitationData, _interactionActivator, _enemyHand);

            return new EnemyAI(_interactionActivator, enemyDragAndDropImitation, _enemyPlayingZone,
                _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw, _bus, _enemyHand, _playerAttackMenu,
                _enemyChoiceMenu);
        }
        
        private void InitPlayersData(SeatPool seatPool)
        {
            _playerHand.Init(seatPool);
            _playerTable.Init();
            _playerPlayingZone.Init(_playerTable);
            _playerTower.Init();
            _playerDiscover.Init();
            _startPlayerTurnLabel.Init();

            SelectNumbersList attackedNumbers = new SelectNumbersList();
            SelectNumbersList choicedNumbers = new SelectNumbersList();

            ConfirmableNumbers confirmableNumbersPlayer = new ConfirmableNumbers(attackedNumbers, choicedNumbers);

            ChoiceResultHandlerPlayer choiceResultHandlerPlayer = new ChoiceResultHandlerPlayer(_informationLabel);

            _playerAttackMenu.Init(_enemyTower, _playerCardAttackZone, CountNumbers, attackedNumbers);
            _playerChoiceMenu.Init(_enemyTower, choiceResultHandlerPlayer, CountNumbers, choicedNumbers);

            _playerCardAttackZone.Init(_playerAttackMenu, _enemyTower);
            //_playerCardAttackZone.Init(_playerChoiceMenu, _enemyTower);
        }

        private void InitEnemyData(SeatPool seatPool)
        {
            _enemyHand.Init(seatPool);
            _enemyTable.Init();
            _enemyPlayingZone.Init(_enemyTable);
            _enemyTower.Init();
            _enemyDiscoverImitation.Init();

            SelectNumbersList attackedNumbers = new SelectNumbersList();
            SelectNumbersList choicedNumbers = new SelectNumbersList();

            ConfirmableNumbers confirmableNumbersEnemyAI = new ConfirmableNumbers(attackedNumbers, choicedNumbers);

            ChoiceResultHandlerPlayer choiceResultHandler = new ChoiceResultHandlerPlayer(_informationLabel);

            _enemyAttackMenu.Init(_playerTower, _enemyCardAttackZone, CountNumbers, attackedNumbers);
            _enemyChoiceMenu.Init(_playerTower, choiceResultHandler, CountNumbers, choicedNumbers);

            _enemyCardAttackZone.Init(_enemyAttackMenu, _playerTower);
        }

        //private void InitCommonData()
        //{
        //    _playerCardAttackZone.Init(_playerAttackMenu, _enemyTower);
        //    _enemyCardAttackZone.Init(_enemyAttackMenu, _playerTower);
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
    }
}