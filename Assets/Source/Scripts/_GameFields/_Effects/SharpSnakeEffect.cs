using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class SharpSnakeEffect : Effect
    {
        private readonly Person _deactivePerson;

        private bool _isEffectComplete;

        public SharpSnakeEffect(Person deactivePerson) : base()
        {
            _deactivePerson = deactivePerson;
            _isEffectComplete = false;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Пироманта закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.ActivateSharpSnakeEffect(CompleteEffect);
            yield return new WaitUntil(() => _isEffectComplete);
        }

        private void CompleteEffect()
        {
            _isEffectComplete = true;
        }
    }
}
