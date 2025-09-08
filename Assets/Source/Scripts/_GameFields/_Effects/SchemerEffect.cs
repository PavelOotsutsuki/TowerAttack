using System.Collections;
using Cards;
using GameFields.Persons.Commons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class SchemerEffect : Effect
    {
        private readonly Person _deactivePerson;
        private readonly Person _activePerson;

        public SchemerEffect(Person activePerson, Person deactivePerson, SignalBus bus, CardEffectData data) : base(bus, data)
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Шулера закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _deactivePerson.ActivateDoubleEffect(1);
            _activePerson.ActivateDoubleEffect(2);
            yield break;
            //yield return new WaitForSeconds(10f);

            //_deactivePerson.AttackDeactivate();
        }
    }
}