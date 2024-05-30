using GameFields.Persons.Discovers;
using GameFields.DiscardPiles;
using GameFields.Persons.Tables;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using GameFields.Persons.DrawCards;

namespace GameFields.Persons
{
    internal class Player : Person
    {
        private Discover _discover;
        private ITableActivator _tableActivator;
        private IBlockable _handBlockable;

        public Player(DiscardPile discardPile, ITableActivator tableActivator, HandPlayer hand, Table table, Tower tower, Discover discover,
            DrawCardRoot drawCardRoot, StartTurnDraw startTurnDraw, TurnProcessing turnProcessing) :
            base(hand, table, drawCardRoot, tower, discardPile, startTurnDraw, turnProcessing)
        {
            _discover = discover;
            _tableActivator = tableActivator;
            _handBlockable = hand;

            _discover.Deactivate();
        }

        protected override void OnStartStep()
        {
            _handBlockable.Block();
            _tableActivator.Activate();
        }
    }
}