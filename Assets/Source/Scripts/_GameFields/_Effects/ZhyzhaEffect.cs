using System.Collections;
using Cards;
using GameFields.Persons;
using UnityEngine;

namespace GameFields.Effects
{
    public class ZhyzhaEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly int _countTurns;

        public ZhyzhaEffect(Person deactivePerson, int countTurns) : base()
        {
            _deactivePerson = deactivePerson;
            _countTurns = countTurns;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Жыжи закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.ActivateSlimeEffect(_countTurns);
            yield break;
            //yield return new WaitForSeconds(10f);

            //_deactivePerson.AttackDeactivate();
        }
    }
}