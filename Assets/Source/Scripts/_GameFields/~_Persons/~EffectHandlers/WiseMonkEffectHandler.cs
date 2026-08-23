using System.Collections.Generic;
using Cards;
using Servers;

namespace GameFields.Persons.EffectHandlers
{
    public class WiseMonkEffectHandler : EffectHandler, IEffectHandlerActiveWatcher, ILengthyEffectHandler
    {
        private readonly List<Card> _effectedCards;

        public WiseMonkEffectHandler(FightProcessDBManager fightProcessDBManager, bool isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        {
            _effectedCards = new List<Card>();
        }

        public bool IsActive => _effectedCards.Count > 0;

        public void Activate(Card card)
        {
            if (_effectedCards.Contains(card))
                return;

            _effectedCards.Add(card);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "ADD", GetType().Name);

            if (_effectedCards.Count == 1)
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, null, "ACTIVATE", GetType().Name);
        }

        public void EndEffect(Card card)
        {
            if (_effectedCards.Contains(card))
            {
                _effectedCards.Remove(card);
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "REMOVE", GetType().Name);

                if (_effectedCards.Count == 0)
                    FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, null, "DEACTIVATE", GetType().Name);
            }
        }
    }
}