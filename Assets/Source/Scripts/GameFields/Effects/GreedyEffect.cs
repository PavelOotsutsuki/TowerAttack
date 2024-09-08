using UnityEngine;
using GameFields.Persons;
using Cards;
using System.Collections;

namespace GameFields.Effects
{
    public class GreedyEffect : Effect
    {
        private Person _activePerson;
        private Person _deactivePerson;

        public GreedyEffect(Person activePerson, Person deactivePerson): base()
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Жадины закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            yield break;
        }
    }
}
