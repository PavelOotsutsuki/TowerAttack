using System.Collections.Generic;
using System.Linq;
using Cards;

namespace GameFields.Persons.EffectHandlers
{
    public class WiseMonkEffectHandler : ILengthyEffectHandler
    {
        private readonly List<Card> _effectedCards;

        public WiseMonkEffectHandler()
        {
            _effectedCards = new List<Card>();
        }

        public bool IsActive => _effectedCards.Count > 0;

        public void Activate(Card card)
        {
            if (_effectedCards.Contains(card))
                return;

            _effectedCards.Add(card);
        }

        public void EndEffect(Card card)
        {
            if (_effectedCards.Contains(card))
                _effectedCards.Remove(card);
        }
    }
}