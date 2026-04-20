using System.Collections;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;
using UnityEngine;

namespace GameFields.Effects
{
    public class GeneralEffect : Effect
    {
        private const int CountNumbers = 4;
        private readonly Person _activePerson;

        public GeneralEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Генерала закончен");
        //}

        protected override IEnumerator OnPlaying()
        {
            bool endChoice = false;

            _activePerson.ChoiceActivate(CountNumbers, () => endChoice = true, RestrictionType.Consecutive);

            yield return new WaitUntil(() => endChoice);
        }
    }
}