using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class JusticeBull_FalseChoiceEffect : Effect
    {
        //private const int CountTurns = 2;
        private readonly Person _activePerson;

        public JusticeBull_FalseChoiceEffect(Person activePerson, SignalBus bus, CardEffectData data) : base(bus, data)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Быка правосудия(2.0) закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _activePerson.ActivateSkipTurns(Duration + 1);

            yield break;
        }
    }
}