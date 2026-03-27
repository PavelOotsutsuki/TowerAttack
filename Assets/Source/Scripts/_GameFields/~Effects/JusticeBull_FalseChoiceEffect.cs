using System.Collections;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class JusticeBull_FalseChoiceEffect : Effect
    {
        //private const int CountTurns = 2;
        private readonly Person _activePerson;
        private readonly Card _card;

        public JusticeBull_FalseChoiceEffect(Person activePerson, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _card = data.CardEffectData.Card;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Быка правосудия(2.0) закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _activePerson.ActivateSkipTurns(_card);

            yield break;
        }
    }
}