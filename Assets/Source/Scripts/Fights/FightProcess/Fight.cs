using Fights.Persons;
using Fights.Persons.Tables;
using Cards;


namespace Fights.FightProcess
{
    internal class Fight : IEndTurnHandler
    {
        private Person _player;
        private Person _enemy;
        private Deck _deck;

        private Person _activePerson;

        public Fight(Person player, Person enemy, Deck deck)
        {
            _player = player;
            _enemy = enemy;
            _deck = deck;

            SetPlayerTurn();
        }

        //public void PlayCard(Card card)
        //{
        //    _activePerson.RemoveCard(card);
        //}

        public void OnEndTurn()
        {
            SwitchPerson();
            StartTurn();
        }

        private void SwitchPerson()
        {
            if (_activePerson == _player)
            {
                SetEnemyTurn();
            }
            else
            {
                SetPlayerTurn();
            }

            ActivateTable();
        }

        private void SetPlayerTurn()
        {
            _activePerson = _player;
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;
        }

        private void StartTurn()
        {
            for (int i = 0; i < _activePerson.CountDrawCards; i++)
            {
                DrawCards();
            }
        }

        private void ActivateTable()
        {
            DeactivateTables();

            _activePerson.ActivateTable();
        }

        private void DeactivateTables()
        {
            _player.DeactivateTable();
            _enemy.DeactivateTable();
        }

        private void DrawCards()
        {
            if (_deck.TryTakeCard(out Card drawnCard))
            {
                _activePerson.DrawCard(drawnCard);
            }
        }
    }
}
