using Cards;
using System.Collections.Generic;
using GameFields.Persons.DrawCards;

namespace GameFields.Persons.EffectHandlers.Fires
{
    public class FireEffectHandler : IEffectHandlerActiveWatcher, ILengthyEffectHandler
    {
        private readonly IFireDrawCardAnimationSetter _cardAnimationManager;

        private readonly List<Card> _effectedCards;

        public FireEffectHandler(IFireDrawCardAnimationSetter cardAnimationManager)
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

            _cardAnimationManager.SetFireMode();
        }

        public void EndEffect(Card card)
        {
            if (_effectedCards.Contains(card) == false)
                return;

            _effectedCards.Remove(card);

            if (_effectedCards.Count == 0)
                _cardAnimationManager.SetSimpleMode();
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