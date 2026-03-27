using System.Collections;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class ThreeGuysEffect : Effect
    {
        private const int CountNumbers = 3;
        private readonly Person _activePerson;

        public ThreeGuysEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Трех Бугаев закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            bool endChoice = false;

            _activePerson.ChoiceImitationActivate(CountNumbers, () => endChoice = true);

            yield return new WaitUntil(() => endChoice);
        }
    }
}