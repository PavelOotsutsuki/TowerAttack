using GameFields.Persons;
using Cards;
using System.Collections;
using UnityEngine;
using GameFields.EndTurnButtons;
using GameFields.DiscardPiles;
using Cysharp.Threading.Tasks;
using GameFields.StartTowerCardSelections;
using GameFields.Effects;

namespace GameFields
{
    internal class Fight : IEndTurnHandler, IStartFightListener
    {
        private readonly int _maxTurns = 100;

        private Player _player;
        private EnemyAI _enemy;
        private Deck _deck;
        private EndTurnButton _endTurnButton;
        private DiscardPile _discardPile;
        private FightAnimator _fightAnimator;
        private StartTowerCardSelection _startTowerCardSelection;
        private EffectRoot _effectRoot;

        private IPerson _activePerson;
        private IPerson _deactivePerson;
        private int _turnNumber;

        public Fight(Player player, EnemyAI enemy, Deck deck, DiscardPile discardPile, EndTurnButton endTurnButton, FightAnimator fightAnimator, StartTowerCardSelection startTowerCardSelection, EffectRoot effectRoot)
        {
            _turnNumber = 1;

            _player = player;
            _enemy = enemy;
            _deck = deck;
            _discardPile = discardPile;
            _endTurnButton = endTurnButton;
            _fightAnimator = fightAnimator;
            _startTowerCardSelection = startTowerCardSelection;
            _effectRoot = effectRoot;

            //_effectRoot.Init(_deck, _discardPile, _activePerson, _deactivePerson);
        }

        public void OnEndTurn()
        {
            _turnNumber++;

            DiscardCards();
            CheckEndFight();
            SwitchPerson();
            StartTurn();
        }

        public void StartFight()
        {
            _startTowerCardSelection.Deactivate();

            SetPlayerTurn();

            _effectRoot.Init(_deck, _discardPile, _activePerson, _deactivePerson);
        }

        public void StartFirstTurn()
        {
            _startTowerCardSelection.Activate(_player, _enemy, _deck);
        }

        private void DiscardCards()
        {
            _fightAnimator.DiscardCards(_activePerson.GetDiscardCards());
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
            if (_activePerson is Player)
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
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;
            _deactivePerson = _player;

            _player.DeactivateDropPlaces();
        }

        private void StartTurn()
        {
            DrawningCard().ToUniTask();
        }

        private void EndFight()
        {
            Debug.Log("БОЙ ОКОНЧЕН! НИЧЬЯ!");
        }

        private IEnumerator DrawningCard()
        {
            WaitForSeconds delay = new WaitForSeconds(_activePerson.DrawCardsDelay);
            Card drawnCard;

            for (int i = 0; i < _activePerson.CountDrawCards; i++)
            {
                if (_deck.IsHasCards(1))
                {
                    drawnCard = _deck.TakeTopCard();
                    _activePerson.DrawCard(drawnCard);
                    yield return delay;
                }
            }

            if (_activePerson is EnemyAI)
            {
                _enemy.PlayDragAndDropImitation();
            }
            else
            {
                _endTurnButton.SetActiveSide();
            }
        }
    }
}
