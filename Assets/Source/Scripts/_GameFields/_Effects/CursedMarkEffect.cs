using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;

namespace GameFields.Effects
{
    public class CursedMarkEffect : Effect
    {
        private readonly Person _deactivePerson;

        public CursedMarkEffect(Person deactivePerson, int countTurns) : base(countTurns)
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
            _deactivePerson.ActivateSlimeEffect(Duration);
            yield break;
            //yield return new WaitForSeconds(10f);

            //_deactivePerson.AttackDeactivate();
        }
    }
}