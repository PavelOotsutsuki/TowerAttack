using System.Collections;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;
using UnityEngine;

namespace GameFields.Effects
{
    public class BlindOldManEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        public BlindOldManEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект слепого старца закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            bool endPlaying = false;

            _activePerson.ChoiceActivate(CountNumbers, () => endPlaying = true, RestrictionType.Odd);
            yield return new WaitUntil(() => endPlaying);
        }
    }
}