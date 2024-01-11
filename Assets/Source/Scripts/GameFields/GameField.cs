using Cards;
using Tools;
using UnityEngine;
using GameFields.Persons;
using UnityEngine.UI;

namespace GameFields
{
    public class GameField : MonoBehaviour
    {
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;
        [SerializeField] private Player _player;
        [SerializeField] private Fight _fight;
        [SerializeField] private CanvasScaler _canvasScaler;

        [SerializeField] private EnemyAI _enemyAI;
        //private Fight _fight;

        public void Init(Card[] cardsInDeck)
        {
            InitDeck(cardsInDeck);
            InitPersons();

            //_fight = new Fight(_player, _enemyAI, _deck);
            _fight.Init(_player, _enemyAI, _deck);

            InitEndTurnButton();
        }

        private void InitPersons()
        {
            _player.Init(_canvasScaler);
            _enemyAI.Init();
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
            DefinePlayer();
            DefineFight();
            DefineCanvasScaler();
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

    }
}
