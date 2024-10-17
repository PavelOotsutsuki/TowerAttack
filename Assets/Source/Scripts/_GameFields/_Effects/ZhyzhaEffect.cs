using System.Collections;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : Effect
    {
        private Person _deactivePerson;

        public ZhyzhaEffect(Person deactivePerson) : base()
        {
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Жыжи закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.AttackActivate();

            yield return new WaitForSeconds(10f);

            _deactivePerson.AttackDeactivate();
        }
    }
}
