using GameFields.Persons;
using Cards;
using System.Collections;
using UnityEngine;
using GameFields.EndTurnButtons;
using GameFields.DiscardPiles;
using Tools;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;

namespace GameFields
{
    internal class Fight : MonoBehaviour, IEndTurnHandler
    {
        private readonly int _maxTurns = 100;

        [SerializeField] private FightAnimator _fightAnimator;

        private Player _player;
        private EnemyAI _enemy;
        private Deck _deck;
        private EndTurnButton _endTurnButton;
        private DiscardPile _discardPile;

        private IPerson _activePerson;
        private int _turnNumber;

        //public Fight(Player player, EnemyAI enemy, Deck deck)
        //{
        //    _turnNumber = 1;

        //    _player = player;
        //    _enemy = enemy;
        //    _deck = deck;

        //    SetPlayerTurn();
        //}

        public void Init(Player player, EnemyAI enemy, Deck deck, DiscardPile discardPile, EndTurnButton endTurnButton)
        {
            _turnNumber = 1;

            _player = player;
            _enemy = enemy;
            _deck = deck;
            _discardPile = discardPile;
            _endTurnButton = endTurnButton;

            _fightAnimator.Init(_discardPile);

            SetPlayerTurn();
        }

        public void OnEndTurn()
        {
            _turnNumber++;

            DiscardCards();
            CheckEndFight();
            SwitchPerson();
            StartTurn();
        }

        private void DiscardCards()
        {
            _fightAnimator.DiscardCards(_activePerson.GetDiscardCards()).ToUniTask();
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

            _player.ActivateDropPlaces();
        }

        private void SetEnemyTurn()
        {
            _activePerson = _enemy;

            _player.DeativateDropPlaces();
        }

        private void StartTurn()
        {
            //StartCoroutine(DrawningCards());
            DrawningCards().ToUniTask();
        }

        private void EndFight()
        {
            Debug.Log("БОЙ ОКОНЧЕН! НИЧЬЯ!");
        }

        private IEnumerator DrawningCards()
        {
            WaitForSeconds delay = new WaitForSeconds(_activePerson.DrawCardsDelay);

            for (int i = 0; i < _activePerson.CountDrawCards; i++)
            {
                if (_deck.TryTakeCard(out Card drawnCard))
                {
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

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFightAnimator();
        }

        [ContextMenu(nameof(DefineFightAnimator))]
        private void DefineFightAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fightAnimator, ComponentLocationTypes.InThis);
        }

    }
}
