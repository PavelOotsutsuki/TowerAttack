using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;

namespace GameFields.Persons
{
    internal class EnemyAI : IPerson
    {
        private HandAI _hand;
        private TableAI _table;
        private TowerAI _tower;

        public EnemyAI(HandAI hand, TableAI table, TowerAI tower, int countDrawCards, float drawCardsDelay)
        {
            CountDrawCards = countDrawCards;
            DrawCardsDelay = drawCardsDelay;
            _hand = hand;
            _table = table;
            _tower = tower;

            _hand.Init();
            _table.Init(this);
            _tower.Init(this);
        }

        public float DrawCardsDelay { get; private set; }
        public int CountDrawCards { get; private set; }

        public void DrawCard(Card card)
        {
            _hand.AddCard(card);
        }

        public void PlayCard(Card card)
        {

        }
    }
}
