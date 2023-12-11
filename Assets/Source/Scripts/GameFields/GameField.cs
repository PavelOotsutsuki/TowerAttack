using Cards;
using Tables;
using Hands;
using GameFields.FightProcess;
using Tools;
using UnityEngine;
using Persons;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Person _player;
        [SerializeField] private Person _enemyAI;

        private Fight _fight;

        public void Init(Card[] cardsInDeck)
        {
            _fight = new Fight(_player, _enemyAI, _deck);

            InitDeck(cardsInDeck);
            InitEndTurnButton();
            InitPersons();
        }

        private void InitPersons()
        {
            _player.Init(_fight);
            _enemyAI.Init(_fight);
        }

        private void InitDeck(Card[] cards)
        {
            _deck.Init(cards);
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
    }
}
