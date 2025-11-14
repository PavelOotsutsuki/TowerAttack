using System.Collections;
using Cards;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class GeneralEffect : Effect
    {
        private const int CountNumbers = 4;
        private readonly Person _activePerson;

        private bool _endPlaying;

        public GeneralEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

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