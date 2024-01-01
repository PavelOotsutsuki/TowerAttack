using GameFields.Persons;
using Cards;


namespace GameFields.FightProcess
{
    internal class Fight_2 : IEndTurnHandler
    {
        private readonly int _maxTurns = 100;

        private Player_2 _player;
        private EnemyAI_2 _enemy;
        private Deck _deck;

        private Person_2 _activePerson;
        private int _turnNumber;

        public Fight_2(Player_2 player, EnemyAI_2 enemy, Deck deck)
        {
            _turnNumber = 1;

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
            _turnNumber++;

            CheckEndFight();
            SwitchPerson();
            StartTurn();
        }

        private void CheckEndFight()
        {
            if (_turnNumber >= _maxTurns)
            {
                EndFight();
            }
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

            ActivateTable();
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;

            DeactivateTable();
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
            _player.ActivateTable();
        }

        private void DeactivateTable()
        {
            _player.DeactivateTable();
        }

        private void EndFight()
        {

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
