using UnityEngine;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class GreedyEffect : Effect
    {
        private Person _activePerson;
        private Person _deactivePerson;

        public GreedyEffect(Person activePerson, Person deactivePerson) 
            : base(EffectTarget.Self)
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;

            PlayEffect();
        }

        private void PlayEffect()
        {
            Debug.Log("Эффект Жадины");
        }
    }
}
