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

            card = TakeTopCard();

            return true;
        }

        public bool TryTakeCards(out Card[] cards, int count)
        {
            if (_cards.Count < count)
            {
                Debug.LogError("Недостаточно карт в колоде");
                cards = null;
                return false;
            }

            cards = new Card[count];

            for (int i = 0; i < count; i++)
            {
                cards[i] = TakeTopCard();
            }

            return true;
        }

        public void AddCard(Card card)
        {
            int position = Random.Range(0, _cards.Count);
            _cards.Insert(position, card);
            ShowCard(card);

            ShuffleCards();
        }

        private Card TakeTopCard()
        {
            return TakeCardByIndex(_cards.Count - 1);
        }

        private Card TakeCardByIndex(int index)
        {
            if (index >= _cards.Count || index < 0)
            {
                Debug.LogError("Invalid card index");
            }

            Card card = _cards[index];

            _cards.Remove(card);

            return card;
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
