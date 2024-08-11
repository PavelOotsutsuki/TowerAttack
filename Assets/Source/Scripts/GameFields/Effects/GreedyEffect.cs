using UnityEngine;
using GameFields.Persons;
using Cards;

namespace GameFields.Effects
{
    public class GreedyEffect : IEffect
    {
        private Person _activePerson;
        private Person _deactivePerson;

        public GreedyEffect(Person activePerson, Person deactivePerson) 
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;
        }

        public void Play()
        {
            Debug.Log("Эффект Жадины");
        }

        public void End()
        {
            Debug.Log("Эффект Жадины закончен");
        }
    }
}
