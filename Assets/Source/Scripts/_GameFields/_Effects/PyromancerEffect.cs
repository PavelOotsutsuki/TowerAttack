using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class PyromancerEffect : Effect
    {
        private readonly Person _deactivePerson;

        public PyromancerEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Пироманта закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.ActivateFireDraw(Duration);
            yield break;
        }
    }
}
