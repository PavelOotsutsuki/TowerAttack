using Cards;
using GameFields.Persons.Tables;

namespace GameFields.Persons.Commons
{
    public class DiscardManager: IDiscardManager
    {
        private readonly IDiscardManager _enemytable;
        private readonly IDiscardManager _playertable;

        public DiscardManager(IDiscardManager tableEnemy, IDiscardManager tablePlayer)
        {
            _enemytable = tableEnemy;
            _playertable = tablePlayer;
        }

        public void Discard(Card card)
        {
            if (_enemytable.HasCard(card))
            {
                _enemytable.Discard(card);
            }

            if (_playertable.HasCard(card))
            {
                _playertable.Discard(card);
            }
        }

        public bool HasCard(Card card)
        {
            return _enemytable.HasCard(card) || _playertable.HasCard(card);
        }
    }
}