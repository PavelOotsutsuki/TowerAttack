using Cards;
using Tools;
using UnityEngine;
using GameFields.Persons;
using UnityEngine.UI;
using GameFields.EndTurnButtons;
using GameFields.DiscardPiles;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Player _player;
        [SerializeField] private Fight _fight;
        [SerializeField] private DiscardPile _discardPile;
        [SerializeField] private CanvasScaler _canvasScaler;

        [SerializeField] private EnemyAI _enemyAI;
        //private Fight _fight;

        public void Init(Card[] cardsInDeck)
        {
            InitDeck(cardsInDeck);
            InitDiscardPile();
            InitPersons();

            //_fight = new Fight(_player, _enemyAI, _deck);
            InitEndTurnButton();

            _fight.Init(_player, _enemyAI, _deck, _discardPile, _endTurnButton);
        }

        private void InitPersons()
        {
            _player.Init(_canvasScaler);
            _enemyAI.Init(_fight, _canvasScaler);
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
            DefineFight();
            DefineCanvasScaler();
            DefineDiscardPile();
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

        [ContextMenu(nameof(DefineFight))]
        private void DefineFight()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fight, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCanvasScaler))]
        private void DefineCanvasScaler()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasScaler, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineDiscardPile))]
        private void DefineDiscardPile()
        {
            AutomaticFillComponents.DefineComponent(this, ref _discardPile, ComponentLocationTypes.InChildren);
        }
    }
}
