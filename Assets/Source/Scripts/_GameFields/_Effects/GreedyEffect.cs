using UnityEngine;
using GameFields.Persons;
using Cards;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.CardTransits;

namespace GameFields.Effects
{
    public class GreedyEffect : Effect
    {
        //private Person _activePerson;
        //private Person _deactivePerson;

        private IHandTransitGetAll _activeHandTransitGetAll;
        private IHandTransitGetAll _deactiveHandTransitGetAll;
        private IHandTransitSet _activeHandTransitSet;
        private IHandTransitSet _deactiveHandTransitSet;

        public GreedyEffect(Person activePerson, Person deactivePerson): base()
        {
            //_activePerson = activePerson;
            //_deactivePerson = deactivePerson;

            _activeHandTransitGetAll = activePerson;
            _deactiveHandTransitGetAll = deactivePerson;
            _activeHandTransitSet = activePerson;
            _deactiveHandTransitSet = deactivePerson;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Жадины закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            List<Card> activePersonCards = _activeHandTransitGetAll.Get();
            List<Card> deactivePersonCards = _deactiveHandTransitGetAll.Get();

            GetCards(deactivePersonCards, _activeHandTransitSet);
            GetCards(activePersonCards, _deactiveHandTransitSet);

            yield break;
        }

        private void GetCards(List<Card> givenCards, IHandTransitSet gettedHandTransitSet)
        {
            if (givenCards is not null)
            {
                if (givenCards.Count > 0)
                {
                    foreach (Card card in givenCards)
                    {
                        gettedHandTransitSet.Set(card);
                    }
                }
            }
        }
    }
}
