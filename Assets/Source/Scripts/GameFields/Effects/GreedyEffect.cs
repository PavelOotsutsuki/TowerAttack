using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class GreedyEffect
    {
        private IPerson _activePerson;
        private IPerson _deactivePerson;

        public GreedyEffect(IPerson activePerson, IPerson deactivePerson)
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
