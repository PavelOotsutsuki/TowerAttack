using System.Collections.Generic;
using System.Linq;
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

        public bool HasCharacter(CardCharacter cardCharacter) => _pairs.ContainsKey(cardCharacter);

        public void BindEffect(CardCharacter character,Effect effect) => _pairs2.Add(effect, character);

        public Dictionary<CardCharacter, Card> GetDiscardCards()
        {
            Dictionary<CardCharacter, Card> cards = new();
            var pairs2Temp = new Dictionary<Effect, CardCharacter>(_pairs2);

            foreach ((Effect effect, CardCharacter character) in pairs2Temp)
            {
                if (effect.CountTurns < 1)
                {
                    cards.Add(character, _pairs[character]);
                    _pairs.Remove(character);
                    _pairs2.Remove(effect);
                }
            }
            
            return cards;
        }

        //public void RemoveCard(IEnumerable<Card> cards)
        //{
        //    foreach (Card card in cards)
        //    {
        //        CardCharacter character = _pairs.FirstOrDefault(kvp => kvp.Value == card).Key;
        //        _pairs.Remove(character);
                
        //        Effect effect = _pairs2.FirstOrDefault(kvp => kvp.Value == character).Key;
        //        _pairs2.Remove(effect);
        //    } 
        //}
    }
}
