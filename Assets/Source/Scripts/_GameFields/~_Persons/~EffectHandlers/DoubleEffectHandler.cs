using System.Collections.Generic;
using Cards;

namespace GameFields.Persons.EffectHandlers
{
    public class DoubleEffectHandler: ILengthyEffectHandler
    {
        private readonly List<Card> _effectedCards;

        public DoubleEffectHandler()
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