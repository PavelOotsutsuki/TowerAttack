using System.Collections.Generic;
using Cards;
using GameFields.Effects;
using GameFields.Persons.Tables;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers.Scarecrows
{
    public class ScarecrowEffectHandler
    {
        private readonly IDiscardManager _discardManager;
        private readonly Queue<ScarecrowEffectData> _effects;

        //private int _counter;
        //private Card _card;

        public ScarecrowEffectHandler(IDiscardManager discardManager)
        {
            //_counter = 0;
            _effects = new Queue<ScarecrowEffectData>();

            _discardManager = discardManager;
        }

        public void Activate(int countCards, Card card)
        {
            _effects.Enqueue(new ScarecrowEffectData(card, countCards));
            //_counter = countCards;
            //_card = card;
        }

        public bool TryUse()
        {
            if (_effects.Count <= 0)
                return false;

            ScarecrowEffectData effect = _effects.Peek();
            effect.Use();

            if (effect.NeedDelete)
            {
                _effects.Dequeue();

                Card card = effect.Card;

                if (_discardManager.HasCard(card))
                {
                    _discardManager.Discard(card);
                }
                else
                {
                    Debug.Log($"Карта {card.name} не найдена");
                }
            }

            return true;
        }
    }
}