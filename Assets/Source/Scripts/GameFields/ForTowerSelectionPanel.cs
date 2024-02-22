using System;
using System.Collections.Generic;
using Cards;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using UnityEngine;

namespace GameFields
{
    public class ForTowerSelectionPanel : MonoBehaviour
    {
        [SerializeField] private Transform[] _seats;
        [SerializeField] private DigPanel _digPanel;

        private List<Card> _cards;

        private void GetCard(Deck deck, Action<Card, IEnumerable<Card>> selectAction)
        {
            List<Card> cards = new();
            
            foreach (Transform seat in _seats)
            {
                if (deck.TryTakeCard(out Card card))
                {
                    card.BindSeat(seat, true, 0f);
                    cards.Add(card);
                }
            }

            CardSelection selection = new(cards, selectAction);
        }
    }

    public class DigPanel : MonoBehaviour
    {
        private CardSelection _cardSelection;
    }

    public class CardSelection
    {
        private readonly List<Card> _cards;
        private readonly Action<Card, IEnumerable<Card>> _selectAction;

        public CardSelection(List<Card> cards, Action<Card, IEnumerable<Card>> selectAction)
        {
            _cards = cards;
            _selectAction = selectAction;
        }

        public void Select(Card card)
        {
            _cards.Remove(card);
            
            _selectAction?.Invoke(card, _cards);
        }
    }

    public class TowerCardSelector
    {
        private Tower _tower;
        private IHand _hand;

        internal TowerCardSelector(Tower tower, IHand hand)
        {
            _tower = tower;
            _hand = hand;
        }

        public void Select(Card selected, IEnumerable<Card> unselected)
        {
            //_tower.Car selected;
        }
    }
}