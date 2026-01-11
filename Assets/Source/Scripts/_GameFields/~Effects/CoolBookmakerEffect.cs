using System.Collections;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;
using UnityEngine;

namespace GameFields.Effects
{
    public class CoolBookmakerEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        public CoolBookmakerEffect(EffectData data) : base(data)
        {
            _activePerson = data.ActivePerson;

            Play();
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Четкого букмекера закончен");
        //}

        protected override IEnumerator OnPlaying()
        {
            bool endChoice = false;
            _activePerson.ChoiceActivate(CountNumbers, () => endChoice = true, RestrictionType.Even);
            yield return new WaitUntil(() => endChoice);
        }
    }
}