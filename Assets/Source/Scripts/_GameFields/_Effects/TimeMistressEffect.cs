using Cards;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Commons;
using Zenject;
using UnityEngine;

namespace GameFields.Effects
{
    public class TimeMistressEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public TimeMistressEffect(Person activePerson, CardLocationViewRoot viewRoot, CardTransitManager transitManager,
             EffectData data) : base(data)
        {
            _activePerson = activePerson;

            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Повелительницы времени закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            TransitToType transitTo = _activePerson is Player ? TransitToType.HandPlayer : TransitToType.HandEnemy;

            if (_viewRoot.TryView(out IReadOnlyList<Card> cards, 1, ViewType.DiscardPile))
            {
                Card card = cards[0];

                _transitManager.TransitCard(card, TransitFromType.DiscardPile, transitTo);
            }

            yield break;
        }
    }
}