using Cards;
using GameFields.Persons;
using System.Collections;
using GameFields.Persons.Discovers;
using UnityEngine;

namespace GameFields.Effects
{
    public class PatriarchCorallEffect: Effect
    {
        private const int CountDrawCards = 3;

        private Deck _deck;
        private Person _activePerson;
        private Person _deactivePerson;
        private Card[] _drawnCards;
        private Discover _discover;
        
        private readonly MonoBehaviour _coroutineContainer;

        public PatriarchCorallEffect(Deck deck, Person activePerson, Person deactivePerson, MonoBehaviour coroutineContainer)
        {
            _deck = deck;
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;
            _coroutineContainer = coroutineContainer;

            PlayEffect();

            CountTurns = 1;
        }

        private void PlayEffect()
        {
            _coroutineContainer.StartCoroutine(Playing());
        }

        private IEnumerator Playing()
        {
            _activePerson.DrawCards(CountDrawCards);

            yield break;
        }
    }
}
