using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using UnityEngine;

namespace GameFields.Persons.DrawCards
{
    public class DrawCardEffect
    {
        private Queue<Card> _drawCards;
        private Action _callback;

        public DrawCardEffect(Queue<Card> drawCards, Action callback)
        {
            _drawCards = drawCards;
            _callback = callback;
        }
    }
}
