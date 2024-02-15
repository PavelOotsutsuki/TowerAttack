using Cards;
using Tools;
using UnityEngine;
using GameFields.Persons;
using GameFields.EndTurnButtons;
using GameFields.DiscardPiles;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Player _player;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private FightAnimator _fightAnimator;

        [SerializeField] private EnemyAI _enemyAI;

        private Fight _fight;

        public void Init(Card[] cardsInDeck)
        {
            _fight = new Fight(_player, _enemyAI, _deck, _discardPile, _endTurnButton, _fightAnimator);

            InitDeck(cardsInDeck);
            InitDiscardPile();
            InitPersons();

            //_fight = new Fight(_player, _enemyAI, _deck);
            InitEndTurnButton();

            _fightAnimator.Init(_discardPile);
        }

        private void InitPersons()
        {
            _player.Init();
            _enemyAI.Init(_fight);
        }

        private void InitDeck(Card[] cards)
        {
            _deck.Init(cards);
        }

        private void InitDiscardPile()
        {
            _discardPile.Init();
        }

        private void InitEndTurnButton()
        {
            _endTurnButton.Init(_fight);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineDeck();
            DefineEndTurnButton();
            DefinePlayer();
            DefineDiscardPile();
            DefineFightAnimator();
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
    }
}
