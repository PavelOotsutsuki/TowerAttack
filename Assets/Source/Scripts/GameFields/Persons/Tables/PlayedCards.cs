using System.Collections.Generic;
using Cards;
using GameFields.Effects;

namespace GameFields.Persons.Tables
{
    public class PlayedCards
    {
        private readonly Dictionary<CardCharacter, Card> _pairs = new();
        private readonly Dictionary<Effect, CardCharacter> _pairs2 = new();

        public CardCharacter GetCharacterByCard(Card card)
        {
            foreach ((CardCharacter character, Card value) in _pairs)
                if (value == card)
                    return character;

            return null;
        }

        public void Add(CardCharacter cardCharacter, Card card) 
            => _pairs.Add(cardCharacter, card);

        public void Remove(CardCharacter cardCharacter) => _pairs.Remove(cardCharacter);

        public bool HasCharacter(CardCharacter cardCharacter) => _pairs.ContainsKey(cardCharacter);

        public void BindEffect(CardCharacter character,Effect effect) => _pairs2.Add(effect, character);

        public List<Card> GetDiscardCards()
        {
            List<Card> cards = new();

            foreach ((Effect effect, CardCharacter character) in _pairs2)
                if (effect.CountTurns < 1)
                    cards.Add(_pairs[character]);

            return cards;
        }
    }
}
