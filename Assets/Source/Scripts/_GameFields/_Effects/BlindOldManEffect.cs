using System.Collections;
using Cards;
using GameFields.Persons;
using GameFields.Persons.SelectMenues.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class BlindOldManEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        private bool _endPlaying;

        public BlindOldManEffect(Person activePerson) : base()
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект слепого старца закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _endPlaying = false;

            _activePerson.ChoiceActivate(CountNumbers, EndPlayingCallback, RestrictionType.Odd);
            yield return new WaitUntil(() => _endPlaying);
        }

        private void EndPlayingCallback()
        {
            _endPlaying = true;
        }
    }
}