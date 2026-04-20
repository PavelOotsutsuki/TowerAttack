using Cards;
using GameFields.Persons.Tables;

namespace GameFields.Persons
{
    public class DiscardManager: IDiscardManager
    {
        private readonly IDiscardManager _enemyTable;
        private readonly IDiscardManager _playerTable;

        public DiscardManager(IDiscardManager tableEnemy, IDiscardManager tablePlayer)
        {
            _enemyTable = tableEnemy;
            _playerTable = tablePlayer;
        }

        public void Discard(Card card)
        {
            if (_enemyTable.HasCard(card))
            {
                _enemyTable.Discard(card);
            }

            if (_playerTable.HasCard(card))
            {
                _playerTable.Discard(card);
            }
        }

        public bool HasCard(Card card)
        {
            return _enemyTable.HasCard(card) || _playerTable.HasCard(card);
        }
    }
}