using Cards;
using System.Collections.Generic;
using GameFields.Persons.DrawCards;
using Servers;

namespace GameFields.Persons.EffectHandlers.Fires
{
    public class FireEffectHandler : EffectHandler, IEffectHandlerActiveWatcher, ILengthyEffectHandler
    {
        private readonly IFireDrawCardAnimationSetter _cardAnimationManager;

        private readonly List<Card> _effectedCards;

        public FireEffectHandler(IFireDrawCardAnimationSetter cardAnimationManager,
            FightProcessDBManager fightProcessDBManager, bool isPlayersObject) : base(fightProcessDBManager, isPlayersObject)
        {
            _cardAnimationManager = cardAnimationManager;
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

            _cardAnimationManager.SetFireMode();
        }

        public void EndEffect(Card card)
        {
            if (_effectedCards.Contains(card) == false)
                return;

            _effectedCards.Remove(card);
            FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, card.ViewData.Number.ToString(), "REMOVE", GetType().Name);

            if (_effectedCards.Count == 0)
            {
                _cardAnimationManager.SetSimpleMode();
                FightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersObject, null, "DEACTIVATE", GetType().Name);
            }
        }

        //public void OnStartTurn()
        //{
        //    if (_countTurns > 0)
        //    {
        //        _countTurns--;

        //        if (_countTurns == 0)
        //        {
        //            _cardAnimationManager.SetSimpleMode();
        //        }
        //    }
        //}
    }
}