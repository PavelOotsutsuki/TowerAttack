using Cards;
using GameFields.Tables;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class GameField : MonoBehaviour, ICardFromHandIntoTableLayable, IDrawCardHandler
    {
        [SerializeField] private Table _table;
        [SerializeField] private Hand _hand;
        [SerializeField] private Deck _deck;
        [SerializeField] private EndTurnButton _endTurnButton;

        public void Init(Card[] cardsInDeck, Card[] cardsInHand = null)
        {
            InitTable();
            InitHand(cardsInHand);
            InitDeck(cardsInDeck);
            InitEndTurnButton();
        }

        public void LayInTableFromHand(Card card)
        {
            _hand.RemoveCard(card);
        }

        public void DrawCard()
        {
            if (_deck.TryTakeCard(out Card drawnCard))
            {
                _hand.AddCard(drawnCard);
            }
        }

        private void InitHand(Card[] cards)
        {
            _hand.Init(cards);
        }

        private void InitTable()
        {
            _table.Init(this);
        }

        private void InitDeck(Card[] cards)
        {
            _deck.Init(cards);
        }

        private void InitEndTurnButton()
        {
            _endTurnButton.Init(this);
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineTable();
            DefineHand();
            DefineDeck();
            DefineEndTurnButton();
        }

        [ContextMenu(nameof(DefineTable))]
        private void DefineTable()
        {
            AutomaticFillComponents.DefineComponent(this, ref _table, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineHand))]
        private void DefineHand()
        {
            AutomaticFillComponents.DefineComponent(this, ref _hand, ComponentLocationTypes.InChildren);
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
