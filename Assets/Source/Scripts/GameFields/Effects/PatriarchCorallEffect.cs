using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using Zenject;
using GameFields.Persons;

namespace GameFields.Effects
{
    public class PatriarchCorallEffect : MonoBehaviour, IEffect
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

        public void Play()
        {
            
        }
    }
}
