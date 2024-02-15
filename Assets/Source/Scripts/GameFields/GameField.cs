using Cards;
using Tools;
using UnityEngine;
using GameFields.Persons;
using GameFields.EndTurnButtons;
using GameFields.DiscardPiles;
using GameFields.FirstTurns;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Player _player;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private FightAnimator _fightAnimator;
        [SerializeField] private FirstTurn _firstTurn;

        [SerializeField] private EnemyAI _enemyAI;

        private Fight _fight;

        public void Init(Card[] cardsInDeck)
        {
            _firstTurn.Init();

            _fight = new Fight(_player, _enemyAI, _deck, _discardPile, _endTurnButton, _fightAnimator, _firstTurn);

            _deck.Init(cardsInDeck);
            _discardPile.Init();
            _player.Init();
            _enemyAI.Init(_fight);
            _endTurnButton.Init(_fight);
            _fightAnimator.Init(_discardPile);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDeck();
            DefineEndTurnButton();
            DefinePlayer();
            DefineDiscardPile();
            DefineFightAnimator();
            DefineFirstTurn();
        }

        [ContextMenu(nameof(DefineDeck))]
        private void DefineDeck()
        {
            AutomaticFillComponents.DefineComponent(this, ref _deck, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineEndTurnButton))]
        private void DefineEndTurnButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _endTurnButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefinePlayer))]
        private void DefinePlayer()
        {
            AutomaticFillComponents.DefineComponent(this, ref _player, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscardPile))]
        private void DefineDiscardPile()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discardPile, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightAnimator))]
        private void DefineFightAnimator()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fightAnimator, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFirstTurn))]
        private void DefineFirstTurn()
        {
            AutomaticFillComponents.DefineComponent(this, ref _firstTurn, ComponentLocationTypes.InChildren);
        }
    }
}
