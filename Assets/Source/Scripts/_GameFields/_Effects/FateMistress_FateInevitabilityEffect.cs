using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class FateMistress_FateInevitabilityEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly int _duration;
        private readonly Card _card;

        public FateMistress_FateInevitabilityEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _duration = data.CardEffectData.Duration;
            _card = data.CardEffectData.Card;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Неизбежность судьбы закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _activePerson.ActivateFateInevitability(_card, _duration);
            yield break;
        }
    }
}