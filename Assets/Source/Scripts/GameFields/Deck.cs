using System.Collections.Generic;
using UnityEngine;
using Cards;

namespace GameFields
{
    public class Deck : MonoBehaviour
    {
        private readonly Vector2 _cardAddPosition = new Vector2(0f, 0f);

        private List<Card> _cards;

        public void Init(IEnumerable<Card> cards)
        {
            _cards = new List<Card>();

            foreach (Card card in cards)
            {
                _cards.Add(card);
                BindCard(card.transform);
            }

            ShuffleCards();
        }

        public bool IsHasCards(int count)
        {
            return _cards.Count >= count;
        }

        public void AddCard(Card card)
        {
            int position = Random.Range(0, _cards.Count);
            _cards.Insert(position, card);
            BindCard(card.transform);

            ShuffleCards();
        }

        public Card TakeTopCard()
        {
            return TakeCardByIndex(_cards.Count - 1);
        }

        private Card TakeCardByIndex(int index)
        {
            Card card = _cards[index];

            _cards.Remove(card);

            return card;
        }

        private void ShuffleCards()
        {
            List<Card> shuffleCards = new List<Card>();

            while(_cards.Count > 0)
            {
                Card card = _cards[Random.Range(0, _cards.Count)];
                shuffleCards.Add(card);
                _cards.Remove(card);
            }

            _cards = shuffleCards;
        }

        private void BindCard(Transform card)
        {
            card.SetParent(transform);
            card.localPosition = _cardAddPosition;
        }
    }
}
