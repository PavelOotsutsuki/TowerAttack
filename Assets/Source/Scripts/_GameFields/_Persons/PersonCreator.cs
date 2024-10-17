using GameFields.EndTurnButtons;
using GameFields.Persons.AttackMenues;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
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

        [SerializeField] private AttackMenu _attackMenu;

        [SerializeField] private StartPlayerTurnLabel _startPlayerTurnLabel; 
        [SerializeField] private int _playerCountStartDrawCards = 1;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("EnemyAI Fields:")]

        [SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;

        private CardPlayingZone _enemyPlayingZone;
        private HandAI _enemyHand;
        private Table _enemyTable;
        private Tower _enemyTower;
        private DiscoverAI _enemyDiscoverImitation;

        [SerializeField] private int _enemyCountStartDrawCards = 1;
        
        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("Table Player Activator:")]

        [SerializeField] private TableActivator _tableActivator;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("Draw Card Animation's data:")]

        [SerializeField] private float _simpleDrawCardDelay = 0.1f;
        [SerializeField] private float _fireDrawCardDelay = 2f;

        private SignalBus _bus;
        private Deck _deck;
        private EndTurnButton _endTurnButton;

        [Inject]
        public void Construct(CardPlayingZonePlayer playerPlayingZone, HandPlayer playerHand, TablePlayer playerTable, TowerPlayer playerTower,
            DiscoverPlayer playerDiscover, CardPlayingZoneAI enemyPlayingZone, HandAI enemyHand, TableAI enemyTable,
            TowerAI enemyTower, DiscoverAI enemyDiscoverImitation)
        {
            _playerPlayingZone = playerPlayingZone;
            _playerHand = playerHand;
            _playerTable = playerTable;
            _playerTower = playerTower;
            _playerDiscover = playerDiscover;
            _enemyPlayingZone = enemyPlayingZone;
            _enemyHand = enemyHand;
            _enemyTable = enemyTable;
            _enemyTower = enemyTower;
            _enemyDiscoverImitation = enemyDiscoverImitation;
        }

        public void Init(SignalBus bus, Deck deck, EndTurnButton endTurnButton, SeatPool seatPool)
        {
            _bus = bus;
            _deck = deck;
            _endTurnButton = endTurnButton;

            InitPlayersData(seatPool);
            InitEnemyData(seatPool);
        }

        public Player CreatePlayer()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay), _deck);
            TurnProcessing turnProcessing = new TurnProcessing(_endTurnButton, _playerHand);
            StartTurnDraw startTurnDraw = new StartTurnDraw(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation, _playerCountStartDrawCards);
            StartPlayerTurnView startPlayerTurnView = new StartPlayerTurnView(_startPlayerTurnLabel);

            return new Player(_tableActivator, _playerHand, _playerPlayingZone, _playerTower, _playerDiscover,
                drawCardRoot, startTurnDraw, turnProcessing, _bus, startPlayerTurnView, _attackMenu);
        }

        public EnemyAI CreateEnemyAI()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay), _deck);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _enemyPlayingZone, _bus);
            StartTurnDraw startTurnDraw = new StartTurnDraw(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation, _enemyCountStartDrawCards);
            
            _enemyDragAndDropImitation.Init(cardDragAndDropImitationActions, _enemyHand);

            return new EnemyAI(_tableActivator, _enemyDragAndDropImitation, _enemyPlayingZone,
                _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw, _bus, _enemyHand, _attackMenu);
        }
        
        private void InitPlayersData(SeatPool seatPool)
        {
            _playerHand.Init(seatPool);
            _playerTable.Init();
            _playerPlayingZone.Init(_playerTable);
            _playerTower.Init();
            _playerDiscover.Init();
            _startPlayerTurnLabel.Init();
            _attackMenu.Init();
        }

        private void InitEnemyData(SeatPool seatPool)
        {
            _enemyHand.Init(seatPool);
            _enemyTable.Init();
            _enemyPlayingZone.Init(_enemyTable);
            _enemyTower.Init();
            _enemyDiscoverImitation.Init();
        }
    }
}
