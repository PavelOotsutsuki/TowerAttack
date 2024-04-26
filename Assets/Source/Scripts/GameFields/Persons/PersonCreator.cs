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
        [SerializeField] private int _playerCountStartTurnDrawCards = 1;
        [SerializeField] private DrawCardRoot _playerDrawCardRoot;
        //[SerializeField] private PlayerSimpleDrawCardAnimation _playerSimpleDrawCardAnimation;
        //[SerializeField] private PlayerDrawCardAnimator _playerDrawCardAnimator;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("EnemyAI Fields:")]

        [SerializeField] private EnemyDragAndDropImitation _enemyDragAndDropImitation;
        [SerializeField] private HandAI _enemyHand;
        [SerializeField] private Table _enemyTable;
        [SerializeField] private Tower _enemyTower;
        [SerializeField] private DrawCardRoot _enemyDrawCardRoot;
        [SerializeField] private DiscoverImitation _enemyDiscoverImitation;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("Table Player Activator:")]

        [SerializeField] private TableActivator _tableActivator;

        private Deck _deck;
        private DiscardPile _discardPile;

        public void Init(Deck deck, DiscardPile discardPile)
        {
            _deck = deck;
            _discardPile = discardPile;
        }

        public void InitPersonsData(EffectRoot effectRoot, SeatPool seatPool)
        {
            InitPlayersData(effectRoot, seatPool);
            InitEnemyData(effectRoot, seatPool);
        }

        public Player CreatePlayer()
        {
            return new Player(_deck, _discardPile, _tableActivator, _playerHand, _playerTable, _playerTower, _playerDiscover, _playerCountStartTurnDrawCards, _playerDrawCardRoot);
        }

        public EnemyAI CreateEnemyAI()
        {
            return new EnemyAI(_deck, _discardPile, _tableActivator, _enemyDragAndDropImitation, _enemyHand, _enemyTable, _enemyTower, _enemyDrawCardRoot, _enemyDiscoverImitation);
        }

        private void InitPlayersData(EffectRoot effectRoot, SeatPool seatPool)
        {
            _playerHand.Init(seatPool);
            _playerTable.Init(_playerHand, effectRoot);
            _playerTower.Init(_playerHand);

            _playerDrawCardRoot.Init(_playerHand);
        }

        private void InitEnemyData(EffectRoot effectRoot, SeatPool seatPool)
        {
            _enemyHand.Init(seatPool);
            _enemyTable.Init(_enemyHand, effectRoot);
            _enemyTower.Init(_enemyHand);

            _enemyDrawCardRoot.Init(_enemyHand);
        }
    }
}
