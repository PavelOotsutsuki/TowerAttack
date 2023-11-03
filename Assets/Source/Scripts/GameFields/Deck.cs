using System.Collections.Generic;
using UnityEngine;
using Cards;

namespace GameFields
{
    public class Deck : MonoBehaviour
    {
        private readonly Vector2 _cardAddPosition = new Vector2(0f, 0f);

        private List<Card> _cards;

        public void Init(List<Card> cards)
        {
            if (cards.Count <= 0)
            {
                _cards = new List<Card>();
                return;
            }

            _cards = cards;

            foreach (Card card in cards)
            {
                ShowCard(card);
            }

            ShuffleCards();
        }

        public void Init(Card[] cards)
        {
            _cards = new List<Card>();

            foreach (Card card in cards)
            {
                _cards.Add(card);
                ShowCard(card);
            }

            ShuffleCards();
        }

        public bool TryTakeCard(out Card card)
        {
            card = null;

            if (_cards.Count <= 0)
            {
                return false;
            }

            card = _cards[_cards.Count - 1];
            card.transform.SetAsLastSibling();
            _cards.Remove(card);

            return true;
        }

        public void AddCard(Card card)
        {
            int position = Random.Range(0, _cards.Count);
            _cards.Insert(position, card);
            ShowCard(card);

            ShuffleCards();
        }

        private void ShuffleCards()
        {
            List<Card> shuffleCards = new List<Card>();
            int index;
            Card card;

            while(_cards.Count > 0)
            {
                index = Random.Range(0, _cards.Count);
                card = _cards[index];
                shuffleCards.Add(card);
                _cards.Remove(card);
            }

            _cards = shuffleCards;
        }

        private void ShowCard(Card card)
        {
            card.transform.SetParent(transform);
            card.transform.localPosition = _cardAddPosition;
        }



    }
}
