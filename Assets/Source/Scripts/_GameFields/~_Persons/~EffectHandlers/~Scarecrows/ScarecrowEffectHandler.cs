using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Effects;
using GameFields.Persons.Tables;
using Servers;
using UnityEngine;

namespace GameFields.Persons.EffectHandlers.Scarecrows
{
    public class ScarecrowEffectHandler: EffectHandler, IEffectHandlerActiveWatcher
    {
        private readonly IDiscardManager _discardManager;
        private readonly Queue<ScarecrowEffectData> _effects;

        //private int _counter;
        //private Card _card;

        public ScarecrowEffectHandler(IDiscardManager discardManager,
            FightProcessDBManager fightProcessDBManager, bool isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        {
            //_counter = 0;
            _effects = new Queue<ScarecrowEffectData>();

            _discardManager = discardManager;
        }

        public bool IsActive => _effects.Count > 0;

        public void Activate(int countCards, Card card)
        {
            if (_effects.Any(e => e.Card == card) == false)
            {
                _effects.Enqueue(new ScarecrowEffectData(card, countCards));
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "ADD CARD", GetType().Name);
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, countCards.ToString(), "countCards", GetType().Name);
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, _effects.Count.ToString(), "COUNT EFFECTS", GetType().Name);
            }
            //_counter = countCards;
            //_card = card;
        }

        public bool TryUse()
        {
            if (_effects.Count <= 0)
                return false;

            ScarecrowEffectData effect = _effects.Peek();
            effect.Use();
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, effect.Card.ViewData.ToString(), "USE", GetType().Name);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, effect.Duration.ToString(), "LEFT ON THIS CARD", GetType().Name);

            if (effect.NeedDelete)
            {
                _effects.Dequeue();
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, effect.Card.ViewData.Number.ToString(), "REMOVE CARD", GetType().Name);

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