using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class FateMistress_FateInevitabilityEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly int _duration;

        public FateMistress_FateInevitabilityEffect(Person activePerson, int duration) : base(duration)
        {
            _activePerson = activePerson;
            _duration = duration;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Неизбежность судьбы закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _activePerson.ActivateFateInevitability(_duration);
            yield break;
        }
    }
}