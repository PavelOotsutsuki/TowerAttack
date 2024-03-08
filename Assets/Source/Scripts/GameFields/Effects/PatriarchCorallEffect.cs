using UnityEngine;
using Cards;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class PatriarchCorallEffect : CardEffect
    {
        private Deck _deck;
        private IPerson _activePerson;
        private IPerson _deactivePerson;

        public void Init(Deck deck, IPerson activePerson, IPerson deactivePerson)
        {
            _deck = deck;
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;
        }

        public override void Play()
        {
            Debug.Log("Эффект Патриарха Коралла");
        }
    }
}
