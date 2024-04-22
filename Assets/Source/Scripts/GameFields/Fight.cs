using GameFields.Persons;
using Cards;
using System.Collections;
using UnityEngine;
using GameFields.EndTurnButtons;
using Cysharp.Threading.Tasks;
using GameFields.StartTowerCardSelections;
using GameFields.Effects;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IPersonSideListener
    {
        private readonly int _maxTurns = 100;

        private Player _player;
        private EnemyAI _enemy;
        //private Deck _deck;
        private EndTurnButton _endTurnButton;
        //private DiscardPile _discardPile;
        //private FightAnimator _fightAnimator;

        private IPerson _activePerson;
        private IPerson _deactivePerson;
        private int _turnNumber;
        private bool _isComlpete;
        private EndFightResults _endFightResults;
       
        public Fight(Player player, EnemyAI enemy, EndTurnButton endTurnButton)
        {
            _isComlpete = false;
            _turnNumber = 1;

            _player = player;
            _enemy = enemy;
            //_deck = deck;
            //_discardPile = discardPile;
            _endTurnButton = endTurnButton;
            //_fightAnimator = fightAnimator;
            //_effectRoot.Init(_deck, _discardPile, _activePerson, _deactivePerson);
        }

        public IPerson ActivePerson => _activePerson;
        public IPerson DeactivePerson => _deactivePerson;
        public bool IsComplete => _isComlpete;
        public EndFightResults EndFightResults => _endFightResults;

        public void OnEndTurn()
        {
            _turnNumber++;

            DiscardCards();
            CheckEndFight();
            SwitchPerson();
            //StartTurn();
        }

        public void Start()
        {
            SetPlayerTurn();
        }

        private void DiscardCards()
        {
            _activePerson.DiscardCards();
        }

        private void CheckEndFight()
        {
            if (_turnNumber >= _maxTurns)
            {
                _endFightResults = EndFightResults.Draw;
                _isComlpete = true;
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
        }

        private void SetPlayerTurn()
        {
            _activePerson = _player;
            _deactivePerson = _enemy;

            _player.ActivateDropPlaces();

            _activePerson.StartTurnDraw();
            _endTurnButton.SetActiveSide();
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;
            _deactivePerson = _player;

            _player.DeactivateDropPlaces();

            _activePerson.StartTurnDraw();

            WaitingEndEnemyImitation().ToUniTask();
        }

        private IEnumerator WaitingEndEnemyImitation()
        {
            yield return new WaitUntil(() => _enemy.IsImitationComplete);

            OnEndTurn();
        }

        //private void StartTurn()
        //{
        //    DrawningCard().ToUniTask();
        //}

        //private void StartTurn()
        //{
        //    _activePerson.StartTurnDraw();
        //    _endTurnButton.SetActiveSide();

        //    //yield return new WaitForSeconds(1.5f);

        //    //if (_activePerson is EnemyAI)
        //    //{
        //    //    _enemy.PlayDragAndDropImitation();
        //    //}
        //    //else
        //    //{
        //    //    _endTurnButton.SetActiveSide();
        //    //}
        //    //if (_activePerson is Player)
        //    //{
        //    //    _endTurnButton.SetActiveSide();
        //    //}
        //}

        //private IEnumerator DrawningCard()
        //{
        //    //WaitForSeconds delay = new WaitForSeconds(_activePerson.DrawCardsDelay);
        //    //Card drawnCard;

        //    //for (int i = 0; i < _activePerson.CountDrawCards; i++)
        //    //{
        //    //    if (_deck.IsHasCards(1))
        //    //    {
        //    //        drawnCard = _deck.TakeTopCard();
        //    //        _activePerson.DrawCard(drawnCard);
        //    //        yield return delay;
        //    //    }
        //    //}

        //    if (_activePerson is EnemyAI)
        //    {
        //        _enemy.PlayDragAndDropImitation();
        //    }
        //    else
        //    {
        //        _endTurnButton.SetActiveSide();
        //    }
        //}
    }
}
