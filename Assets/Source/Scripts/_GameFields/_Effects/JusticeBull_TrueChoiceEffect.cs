using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class JusticeBull_TrueChoiceEffect : Effect
    {
        private const int CountNumbers = 2;
        private readonly Person _activePerson;

        private bool _endPlaying;

        public JusticeBull_TrueChoiceEffect(Person activePerson, SignalBus bus, CardEffectData data) : base(bus, data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Быка правосудия(2.1) закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _endPlaying = false;

            _activePerson.AttackActivate(CountNumbers, EndPlayingCallback);

            yield return new WaitUntil(() => _endPlaying);
        }

        private void EndPlayingCallback()
        {
            _endPlaying = true;
        }
    }
}