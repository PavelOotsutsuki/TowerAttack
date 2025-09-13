using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : Effect
    {
        private readonly Person _deactivePerson;

        public ZhyzhaEffect(Person deactivePerson, EffectData data) : base(data)
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Жыжи закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.ActivateSlimeEffect(Duration);
            yield break;
            //yield return new WaitForSeconds(10f);

            //_deactivePerson.AttackDeactivate();
        }
    }
}