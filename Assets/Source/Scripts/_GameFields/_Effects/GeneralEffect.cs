using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class GeneralEffect : Effect
    {
        private const int CountNumbers = 4;
        private readonly Person _activePerson;

        private bool _endPlaying;

        public GeneralEffect(Person activePerson) : base()
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Генерала закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _endPlaying = false;

            _activePerson.ChoiceActivate(CountNumbers, EndPlayingCallback, RestrictionType.Consecutive);

            yield return new WaitUntil(() => _endPlaying);
        }

        private void EndPlayingCallback()
        {
            _endPlaying = true;
        }
    }
}