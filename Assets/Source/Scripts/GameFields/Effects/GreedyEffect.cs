using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class GreedyEffect : CardEffect
    {
        private IPerson _activePerson;
        private IPerson _deactivePerson;

        public void Init(IPerson activePerson, IPerson deactivePerson)
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;
        }

        public override void Play()
        {
            Debug.Log("Эффект жадины");
        }
    }
}
