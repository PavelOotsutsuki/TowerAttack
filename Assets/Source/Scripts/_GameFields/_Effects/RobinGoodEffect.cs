using System.Collections;
using System.Linq;
using Cards;
using GameFields.Persons.Commons;
using GameFields.Persons.DrawCards;
using UnityEngine;

namespace GameFields.Effects
{
    public class RobinGoodEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly Person _deactivePerson;

        private bool _isEffectComplete;

        private readonly CardLocationViewRoot _cardLocationViewRoot;

        public RobinGoodEffect(Person activePerson, Person deactivePerson, CardLocationViewRoot cardLocationViewRoot) : base()
        {
            _activePerson = activePerson;
            _deactivePerson = deactivePerson;
            _isEffectComplete = false;

            _cardLocationViewRoot = cardLocationViewRoot;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Роибн Гуда закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            Person player;
            Person enemyAI;

            if (_activePerson is Player)
            {
                player = _activePerson;
                enemyAI = _deactivePerson;
            }
            else
            {
                player = _deactivePerson;
                enemyAI = _activePerson;
            }

            int countHandAI = _cardLocationViewRoot.GetAllCards(ViewType.HandAI).Count();
            int countHandPlayer = _cardLocationViewRoot.GetAllCards(ViewType.HandPlayer).Count();
            int countCards = countHandPlayer - countHandAI;

            if (countCards == 0)
            {
                CompleteEffect();
                yield break;
            }

            IDrawCardManager gettedPerson;

            if (countCards > 0)
            {
                gettedPerson = enemyAI;
            }
            else
            {
                countCards *= -1;
                gettedPerson = player;
            }

            gettedPerson.DrawCards(countCards, CompleteEffect);

            yield return new WaitUntil(() => _isEffectComplete);
        }

        private void CompleteEffect()
        {
            _isEffectComplete = true;
        }
    }
}