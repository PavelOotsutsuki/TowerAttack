using GameFields.Effects;
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

        public void Init(SignalBus bus, SeatPool seatPool, EffectFactory effectFactory)
        {
            _bus = bus;

            InitPlayersData(seatPool, effectFactory);
            InitEnemyData(seatPool, effectFactory);
        }

        public Player CreatePlayer(SignalBus bus, Deck deck, EndTurnButton endTurnButton)
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay), deck);
            TurnProcessing turnProcessing = new TurnProcessing(endTurnButton, _playerHand);
            StartTurnDraw startTurnDraw = new StartTurnDraw(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation, _playerCountStartDrawCards);

            return new Player(_tableActivator, _playerHand, _playerPlayingZone, _playerTower, _playerDiscover,
                drawCardRoot, startTurnDraw, turnProcessing, bus);
        }

        public EnemyAI CreateEnemyAI(SignalBus bus, Deck deck)
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay), deck);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _enemyPlayingZone);
            StartTurnDraw startTurnDraw = new StartTurnDraw(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation, _enemyCountStartDrawCards);
            
            _enemyDragAndDropImitation.Init(cardDragAndDropImitationActions, _enemyHand);

            return new EnemyAI(_tableActivator, _enemyDragAndDropImitation, _enemyPlayingZone,
                _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw, bus);
        }
        
        private void InitPlayersData(SeatPool seatPool, EffectFactory effectFactory)
        {
            _playerHand.Init(seatPool);
            _playerTable.Init();
            _playerPlayingZone.Init(_playerTable, _bus, effectFactory);
            _playerTower.Init();
        }

        private void InitEnemyData(SeatPool seatPool, EffectFactory effectFactory)
        {
            _enemyHand.Init(seatPool);
            _enemyTable.Init();
            _enemyPlayingZone.Init(_enemyTable, _bus, effectFactory);
            _enemyTower.Init();
        }
    }
}
