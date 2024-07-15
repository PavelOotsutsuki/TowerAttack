using Cards;
using GameFields.DiscardPiles;
using GameFields.Effects;
using GameFields.EndTurnButtons;
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
        private EndTurnButton _endTurnButton;

        public void Init(DiscardPile discardPile, Deck deck, EndTurnButton endTurnButton)
        {
            _discardPile = discardPile;
            _deck = deck;
            _endTurnButton = endTurnButton;
        }

        public void InitPersonsData(EffectRoot effectRoot, SeatPool seatPool)
        {
            InitPlayersData(effectRoot, seatPool);
            InitEnemyData(effectRoot, seatPool);
        }

        public Player CreatePlayer()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_playerHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay), _deck);

            TurnProcessing turnProcessing = new TurnProcessing(_endTurnButton, _playerHand);

            StartTurnDraw startTurnDraw = new StartTurnDraw(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation, _playerCountStartDrawCards);

            return new Player(_discardPile, _tableActivator, _playerHand, _playerTable, _playerTower, _playerDiscover, drawCardRoot, startTurnDraw, turnProcessing);
        }

        public EnemyAI CreateEnemyAI()
        {
            SimpleDrawCardAnimation simpleDrawCardAnimation = new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay);
            FireDrawCardAnimation fireDrawCardAnimation = new FireDrawCardAnimation(_enemyHand, _fireDrawCardDelay);
            DrawCardRoot drawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay), _deck);
            CardDragAndDropImitationActions cardDragAndDropImitationActions = new CardDragAndDropImitationActions(_enemyHand, _enemyTable);

            _enemyDragAndDropImitation.Init(cardDragAndDropImitationActions, _enemyHand);

            StartTurnDraw startTurnDraw = new StartTurnDraw(drawCardRoot, simpleDrawCardAnimation, fireDrawCardAnimation, _enemyCountStartDrawCards);

            return new EnemyAI(_discardPile, _tableActivator, _enemyDragAndDropImitation, _enemyHand, _enemyTable, _enemyTower, drawCardRoot, _enemyDiscoverImitation, startTurnDraw);
        }
        
        private void InitPlayersData(EffectRoot effectRoot, SeatPool seatPool)
        {
            _playerHand.Init(seatPool);
            _playerTable.Init(_playerHand);
            _playerTower.Init(_playerHand);

            //_playerDrawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_playerHand, _simpleDrawCardDelay), deck);
        }

        private void InitEnemyData(EffectRoot effectRoot, SeatPool seatPool)
        {
            _enemyHand.Init(seatPool);
            _enemyTable.Init(_enemyHand);
            _enemyTower.Init(_enemyHand);

            //_enemyDrawCardRoot = new DrawCardRoot(new SimpleDrawCardAnimation(_enemyHand, _simpleDrawCardDelay), deck);
        }
    }
}
