using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using Cysharp.Threading.Tasks;
using System;

namespace GameFields.Effects
{
    public class PatriarchCorallEffect
    {
        private const int CountDrawCards = 3;

        private Deck _deck;
        private IPerson _activePerson;
        private IPerson _deactivePerson;
        private Card[] _drawnCrads;

        public PatriarchCorallEffect(Deck deck, IPerson activePerson, IPerson deactivePerson)
        {
            _deck = deck;
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;

            PlayEffect();
        }

        private void PlayEffect()
        {
            Playing().ToUniTask();
        }

        private IEnumerator Playing()
        {
            _drawnCrads = new Card[CountDrawCards];

            for (int i = 0; i < CountDrawCards; i++)
            {
                if (_deck.IsHasCards(1))
                {
                    yield return DrawingCard(i);
                }
            }
        }

        private IEnumerator DrawingCard(int index)
        {
            Card card = _deck.TakeTopCard();

            _activePerson.DrawCard(card);
            yield return new WaitForSeconds(_activePerson.DrawCardsDelay);

            _drawnCrads[index] = card;
        }
    }
}
