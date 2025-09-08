using Cards;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Zenject;
using UnityEngine;

namespace GameFields.Effects
{
    public class GreedyEffect : Effect
    {
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public GreedyEffect(CardLocationViewRoot viewRoot, CardTransitManager transitManager, SignalBus bus, CardEffectData data)
            : base(bus, data)
        {
            //_activePerson = activePerson;
            //_deactivePerson = deactivePerson;

            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Жадины закончен");
        }

        //protected override IEnumerator OnPlaying()
        //{
        //    _deactivePerson.AttackActivate();
        //    yield break;
        //    //yield return new WaitForSeconds(10f);

        //    //_deactivePerson.AttackDeactivate();
        //}

        protected override IEnumerator OnPlaying()
        {
            IEnumerable<Card> enemyCards = _viewRoot.GetAllCards(ViewType.HandAI);
            IEnumerable<Card> playerCards = _viewRoot.GetAllCards(ViewType.HandPlayer);

            GetCards(playerCards, TransitFromType.HandPlayer, TransitToType.HandEnemy);
            GetCards(enemyCards, TransitFromType.HandEnemy, TransitToType.HandPlayer);

            yield break;
        }

        private void GetCards(IEnumerable<Card> givenCards, TransitFromType fromType, TransitToType toType)
        {
            if (givenCards is not null)
            {
                if (givenCards.Count() > 0)
                {
                    foreach (Card card in givenCards)
                    {
                        _transitManager.TransitCard(card, fromType, toType);
                    }
                }
            }
        }
    }
}