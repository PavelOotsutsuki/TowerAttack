using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using System.Collections.Generic;

namespace GameFields.Effects
{
    public class PatriarchCorallEffect : Effect
    {
        private readonly int _countDrawCards = 3;
        private readonly string _activateDiscoverMessage = "Выберете, какую карту отдадите противнику";
        private readonly float _waitDiscoverDuration = 0.5f;

        private Person _activePerson;
        private Person _deactivePerson;
        private List<Card> _cards;
        private bool _endPlaying;

        public PatriarchCorallEffect(Person activePerson, Person deactivePerson): base()
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            _endPlaying = false;
            _cards = _activePerson?.DrawCards(_countDrawCards, DiscoverCards);

            yield return new WaitUntil(() => _endPlaying);
        }

        public override void End()
        {
            Debug.Log("End patriarch corall effect");
        }

        private void DiscoverCards()
        {
            if (_cards is null)
            {
                _endPlaying = true;
                return;
            }

            if (_cards.Count <= 0)
            {
                _endPlaying = true;
                return;
            }

            _activePerson.DiscoverCards(_cards, _activateDiscoverMessage, RechangeCards, _waitDiscoverDuration);
        }

        private void RechangeCards(Card myCard)
        {
            if (_activePerson.TryGetCard(myCard))
            {
                _deactivePerson.SetCard(myCard);
            }

            _endPlaying = true;
        }
    }
}
