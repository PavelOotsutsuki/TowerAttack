using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using Cysharp.Threading.Tasks;
using System;
using GameFields.Persons.Discovers;
using GameFields.Persons.DrawCards;
using System.Collections.Generic;

namespace GameFields.Effects
{
    public class PatriarchCorallEffect : Effect
    {
        private const int CountDrawCards = 3;

        private Deck _deck;
        private Person _activePerson;
        private Person _deactivePerson;
        private Card[] _drawnCards;
        private Discover _discover;

        public PatriarchCorallEffect(Card card, Deck deck, Person activePerson, Person deactivePerson) 
            : base(card, EffectTarget.Self, 1)
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
            //yield return _activePerson.PatriarchCorallDraw();
            //Queue<Card> cards = new Queue<Card>();

            //for (int i = 0; i < CountDrawCards; i++)
            //{
            //    if (_deck.IsHasCards(1))
            //    {
            //        cards.Enqueue(_deck.TakeTopCard());
            //    }
            //}


            //if (cards != null)
            //{
                _activePerson?.DrawCards(CountDrawCards);
            //}

            yield break;

            //_discover.Activate(cards, )

        }

        //private IEnumerator Playing()
        //{
        //    _drawnCards = new Card[CountDrawCards];

        //    for (int i = 0; i < CountDrawCards; i++)
        //    {
        //        if (_deck.IsHasCards(1))
        //        {
        //            yield return DrawingCard(i);
        //        }
        //    }
        //}

        //private IEnumerator DrawingCard(int index)
        //{
        //    //Card card = _deck.TakeTopCard();

        //    //_activePerson.DrawCard(card);
        //    //yield return new WaitForSeconds(_activePerson.DrawCardsDelay);

        //    //_drawnCards[index] = card;
        //}
    }
}
