using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using Cysharp.Threading.Tasks;
using System;

namespace GameFields.Effects
{
    [CreateAssetMenu(fileName = "New PatriarchCorallEffect", menuName = "SO/Create effect/PatriarchCorallEffect", order = 51)]
    public class PatriarchCorallEffect : CardEffect
    {
        private const int CountDrawCards = 3;

        private Deck _deck;
        private IPerson _activePerson;
        private IPerson _deactivePerson;
        private Card[] _drawnCrads;

        public void Init(Deck deck, IPerson activePerson, IPerson deactivePerson)
        {
            _deck = deck;
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;
        }

        public override void Play()
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
