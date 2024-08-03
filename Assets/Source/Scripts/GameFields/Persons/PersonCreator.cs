using GameFields.EndTurnButtons;
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

        [SerializeField] private CardPlayingZone _playerPlayingZone;
        [SerializeField] private HandPlayer _playerHand;
        [SerializeField] private Table _playerTable;
        [SerializeField] private Tower _playerTower;
        [SerializeField] private Discover _playerDiscover;
        [SerializeField] private int _playerCountStartDrawCards = 1;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("EnemyAI Fields:")]

        [SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        [SerializeField] private CardPlayingZone _enemyPlayingZone;
        [SerializeField] private HandAI _enemyHand;
        [SerializeField] private Table _enemyTable;
        [SerializeField] private Tower _enemyTower;
        [SerializeField] private DiscoverImitation _enemyDiscoverImitation;
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
            SimpleDrawCardAnimation simpleDrawCardAnimation = new(_playerHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new(_playerHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new(new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay), _deck);
            TurnProcessing turnProcessing = new(_endTurnButton, _playerHand);
            StartTurnDraw startTurnDraw = new(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation, _playerCountStartDrawCards);

            return new Player(_tableActivator, _playerHand, _playerPlayingZone, _playerTower, _playerDiscover,
                drawCardRoot, startTurnDraw, turnProcessing, _bus);
        }

        public EnemyAI CreateEnemyAI()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new(_enemyHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new(_enemyHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new(new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay), _deck);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new(_enemyHand, _enemyPlayingZone);
            StartTurnDraw startTurnDraw = new(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation, _enemyCountStartDrawCards);
            
            _enemyDragAndDropImitation.Init(cardDragAndDropImitationActions, _enemyHand);

            return new EnemyAI(_tableActivator, _enemyDragAndDropImitation, _enemyPlayingZone,
                _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw, _bus);
        }
        
        private void InitPlayersData(SeatPool seatPool)
        {
            _playerHand.Init(seatPool);
            _playerTable.Init();
            _playerPlayingZone.Init(_playerTable/*, _playerHand*/);
            _playerTower.Init(/*_playerHand*/);
        }

        private void InitEnemyData(SeatPool seatPool)
        {
            _enemyHand.Init(seatPool);
            _enemyTable.Init();
            _enemyPlayingZone.Init(_enemyTable/*, _enemyHand*/);
            _enemyTower.Init(/*_enemyHand*/);
        }
    }
}
