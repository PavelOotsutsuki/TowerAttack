using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class GreedyEffect : Effect
    {
        private Person _activePerson;
        private Person _deactivePerson;

        public GreedyEffect(Card card, Person activePerson, Person deactivePerson) 
            : base(card, EffectTarget.Self, 1)
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
