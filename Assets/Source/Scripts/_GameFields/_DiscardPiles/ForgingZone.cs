using System.Collections;
using Cards;
using Cards.DependencyInterlayers;
using GameFields.Persons.DrawCards;
using GameFields.Persons.EffectHandlers;
using GameFields.Persons.Hands;
using Tools.Settings;
using UnityEngine;
using Zenject;

namespace GameFields.DiscardPiles
{
    public class ForgingZone : ExtraEffectZone, IForging
    {
        private IDrawCardManager _drawCardManager;
        private GnomeEffectHandler _gnomeEffectHandler;

        public void Init(ICardSeatable cardSeatable, SignalBus signalBus, IDrawCardManager drawCardManager,
            GnomeEffectHandler gnomeEffectHandler)
        {
            base.Init(cardSeatable, signalBus);

            _drawCardManager = drawCardManager;
            _gnomeEffectHandler = gnomeEffectHandler;
        }

        protected override void OnEndProcessing()
        {
            _drawCardManager.DrawCards(1, Continue);
            _gnomeEffectHandler.Upgrade();
        }

        private IEnumerator WaitingUntilComplete()
        {
            yield return new WaitForSeconds(GameSettings.DefaultEffectDelayBeforeComplete);

            IsComplete = true;
        }

        private void Continue()
        {
            StartCoroutine(WaitingUntilComplete());
        }
    }
}