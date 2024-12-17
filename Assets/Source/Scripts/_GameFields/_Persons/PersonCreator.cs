using GameFields.EndTurnButtons;
using GameFields.Persons.AttackMenues;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using Tools;
using UnityEngine;
using Zenject;

namespace GameFields.Persons
{
    public class PersonCreator : MonoBehaviour
    {
        [Header("Player Fields:")]

        private CardPlayingZone _playerPlayingZone;
        private HandPlayer _playerHand;
        private Table _playerTable;
        private Tower _playerTower;
        private DiscoverPlayer _playerDiscover;
        private AttackMenuPlayer _playerAttackMenu;

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
        private AttackMenuImitation _enemyAttackMenu;
        private CardAttackZone _enemyCardAttackZone;

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

        [SerializeField] private float _simpleDrawCardDelay = 0.1f;
        [SerializeField] private float _fireDrawCardDelay = 2f;

        private SignalBus _bus;
        private Deck _deck;
        private EndTurnButton _endTurnButton;

        private GameFieldObjectsActivator _gameFieldObjectsActivator;

        [Inject]
        public void Construct(CardPlayingZonePlayer playerPlayingZone, HandPlayer playerHand, TablePlayer playerTable, TowerPlayer playerTower,
            DiscoverPlayer playerDiscover, AttackMenuPlayer playerAttackMenu, CardPlayingZoneAI enemyPlayingZone, HandAI enemyHand,
            TableAI enemyTable, TowerAI enemyTower, DiscoverAI enemyDiscoverImitation, AttackMenuImitation enemyAttackMenu,
            CardAttackZone enemyCardAttackZone)
        {
            _playerPlayingZone = playerPlayingZone;
            _playerHand = playerHand;
            _playerTable = playerTable;
            _playerTower = playerTower;
            _playerDiscover = playerDiscover;
            _playerAttackMenu = playerAttackMenu;

            _enemyPlayingZone = enemyPlayingZone;
            _enemyHand = enemyHand;
            _enemyTable = enemyTable;
            _enemyTower = enemyTower;
            _enemyDiscoverImitation = enemyDiscoverImitation;
            _enemyAttackMenu = enemyAttackMenu;
            _enemyCardAttackZone = enemyCardAttackZone;
        }

        public void Init(SignalBus bus, Deck deck, EndTurnButton endTurnButton, SeatPool seatPool)
        {
            _bus = bus;
            _deck = deck;
            _endTurnButton = endTurnButton;

            _gameFieldObjectsActivator = new GameFieldObjectsActivator(_playerHand, _towerActivator, _tableActivator, endTurnButton);

            InitPlayersData(seatPool);
            InitEnemyData(seatPool);
        }

        public Player CreatePlayer()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay), _deck);
            TurnProcessing turnProcessing = new TurnProcessing(_gameFieldObjectsActivator);
            StartTurnDrawPlayer startTurnDraw = new StartTurnDrawPlayer(_gameFieldObjectsActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, _playerCountStartDrawCards);
            StartPlayerTurnView startPlayerTurnView = new StartPlayerTurnView(_gameFieldObjectsActivator, _startPlayerTurnLabel);
            EndTurnProcessing endTurnProcessing = new EndTurnProcessing(_endTurnButton, _gameFieldObjectsActivator);

            return new Player(_gameFieldObjectsActivator, _playerHand, _playerPlayingZone, _playerTower, _playerDiscover,
                drawCardRoot, startTurnDraw, turnProcessing, _bus, startPlayerTurnView, _playerAttackMenu, endTurnProcessing);
        }

        public EnemyAI CreateEnemyAI()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay), _deck);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _enemyPlayingZone, _bus);
            StartTurnDrawEnemyAI startTurnDraw = new StartTurnDrawEnemyAI(_gameFieldObjectsActivator, drawCardRoot, simpleDrawCardAnimation,
                fireDrawCardAnimation, _enemyCountStartDrawCards);
            EnemyDragAndDropImitation enemyDragAndDropImitation = new EnemyDragAndDropImitation(cardDragAndDropImitationActions,
                _enemyDragAndDropImitationData, _gameFieldObjectsActivator, _enemyHand);

            return new EnemyAI(_gameFieldObjectsActivator, enemyDragAndDropImitation, _enemyPlayingZone,
                _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw, _bus, _enemyHand, _playerAttackMenu);
        }
        
        private void InitPlayersData(SeatPool seatPool)
        {
            _playerHand.Init(seatPool);
            _playerTable.Init();
            _playerPlayingZone.Init(_playerTable);
            _playerTower.Init();
            _playerDiscover.Init();
            _startPlayerTurnLabel.Init();
            _playerAttackMenu.Init(_playerHand, _enemyTower);
        }

        private void InitEnemyData(SeatPool seatPool)
        {
            _enemyHand.Init(seatPool);
            _enemyTable.Init();
            _enemyPlayingZone.Init(_enemyTable);
            _enemyTower.Init();
            _enemyDiscoverImitation.Init();
            _enemyAttackMenu.Init(_enemyHand, _playerTower);

            _enemyCardAttackZone.Init(_playerAttackMenu, _enemyTower.ReadOnlyRectTransform);
        }
    }
}