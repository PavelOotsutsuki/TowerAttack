using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class JusticeBull_FalseChoiceEffect : Effect
    {
        private const int CountTurns = 2;
        private readonly Person _activePerson;

        public JusticeBull_FalseChoiceEffect(Person activePerson) : base(CountTurns - 1)
        {
            _activePerson = activePerson;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Быка правосудия(2.0) закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _activePerson.ActivateSkipTurns(CountTurns);

            yield break;
        }
    }
}