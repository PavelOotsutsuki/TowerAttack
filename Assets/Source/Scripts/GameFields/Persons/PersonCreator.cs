using GameFields.DiscardPiles;
using GameFields.Effects;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using GameFields.Seats;
using UnityEngine;

namespace GameFields.Persons
{
    internal class PersonCreator: MonoBehaviour
    {
        [Header("Player Fields:")]

        [SerializeField] private HandPlayer _playerHand;
        [SerializeField] private Table _playerTable;
        [SerializeField] private Tower _playerTower;
        [SerializeField] private Discover _playerDiscover;
        //[SerializeField] private DrawCardRoot _playerDrawCardRoot;
        //[SerializeField] private PlayerSimpleDrawCardAnimation _playerSimpleDrawCardAnimation;
        //[SerializeField] private PlayerDrawCardAnimator _playerDrawCardAnimator;
        //[SerializeField] private CardBlockPanel _cardBlockPanel;
        [SerializeField] private int _playerCountStartDrawCards = 1;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("EnemyAI Fields:")]

        [SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        [SerializeField] private HandAI _enemyHand;
        [SerializeField] private Table _enemyTable;
        [SerializeField] private Tower _enemyTower;
        //[SerializeField] private DrawCardRoot _enemyDrawCardRoot;
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

        private DiscardPile _discardPile;
        private Deck _deck;

        public void Init(DiscardPile discardPile, Deck deck)
        {
            _discardPile = discardPile;
            _deck = deck;
        }

        public void InitPersonsData(EffectRoot effectRoot, SeatPool seatPool)
        {
            InitPlayersData(effectRoot, seatPool);
            InitEnemyData(effectRoot, seatPool);
        }

        public Player CreatePlayer()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay, this);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerHand, _fireDrawCardDelay, this);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay, this),
                _deck, this);

            StartTurnDraw startTurnDraw = new StartTurnDraw(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation,
                _playerCountStartDrawCards, this);

            return new Player(_discardPile, _tableActivator, _playerHand, _playerTable, _playerTower, _playerDiscover,
                drawCardRoot, startTurnDraw, this);
        }

        public EnemyAI CreateEnemyAI()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay, this);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyHand, _fireDrawCardDelay, this);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay, this),
                _deck, this);

            StartTurnDraw startTurnDraw = new StartTurnDraw(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation,
                _enemyCountStartDrawCards, this);

            return new EnemyAI(_discardPile, _tableActivator, _enemyDragAndDropImitation, _enemyHand, _enemyTable,
                _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw, this);
        }
        
        private void InitPlayersData(EffectRoot effectRoot, SeatPool seatPool)
        {
            _playerHand.Init(seatPool);
            _playerTable.Init(_playerHand, effectRoot);
            _playerTower.Init(_playerHand);

            //_playerDrawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay), deck);
        }

        private void InitEnemyData(EffectRoot effectRoot, SeatPool seatPool)
        {
            _enemyHand.Init(seatPool);
            _enemyTable.Init(_enemyHand, effectRoot);
            _enemyTower.Init(_enemyHand);

            //_enemyDrawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay), deck);
        }
    }
}
