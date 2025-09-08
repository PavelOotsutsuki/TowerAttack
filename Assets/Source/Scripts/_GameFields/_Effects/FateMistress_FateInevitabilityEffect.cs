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

        public FateMistress_FateInevitabilityEffect(Person activePerson, SignalBus bus, CardEffectData data) : base(bus, data)
        {
            _activePerson = activePerson;
            _duration = data.Duration;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Неизбежность судьбы закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _activePerson.ActivateFateInevitability(_duration);
            yield break;
        }
    }
}