using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    [CreateAssetMenu(fileName = "New GreedyEffect", menuName = "SO/Create effect/GreedyEffect", order = 51)]
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
