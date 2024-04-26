using GameFields.DiscardPiles;
using GameFields.Effects;
using GameFields.Persons.Tables;
using GameFields.Seats;
using UnityEngine;

namespace GameFields.Persons
{
    internal class PersonCreator: MonoBehaviour
    {
        [Header("Player Fields:")]

        [SerializeField] private PlayerData _playerData;

        [Space]
        [Header("----------------------------")]
        [Space]

        [Header("EnemyAI Fields:")]

        [SerializeField] private EnemyAiData _enemyAiData;

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

        public Player CreatePlayer()
        {
            return new Player(_deck, _discardPile, _tableActivator, _playerData);
        }

        public EnemyAI CreateEnemyAI()
        {
            return new EnemyAI(_deck, _discardPile, _tableActivator, _enemyAiData);
        }
    }
}
