using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using Tools;
using UnityEngine;

namespace GameFields.Effects
{
    public class OutOfControlBusEffect : Effect
    {
        private const int CountNumbers = 3;

        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public OutOfControlBusEffect(Person activePerson, CardLocationViewRoot cardLocationViewRoot,
            CardTransitManager transitManager, EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _viewRoot = cardLocationViewRoot;
            _transitManager = transitManager;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект неуправляемого автобуса закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            bool endChoice = false;

            _activePerson.ChoiceActivate(CountNumbers, () => endChoice = true);
            yield return new WaitUntil(() => endChoice);

            IEnumerable<int> newCheckedNumbers = _activePerson.LastSelectedNumbers;

            IEnumerable<Card> handEnemyCards = _viewRoot.GetAllCards(ViewType.HandAI);
            IEnumerable<Card> handPlayerCards = _viewRoot.GetAllCards(ViewType.HandPlayer);

            IEnumerable<Card> firedCardsEnemy = handEnemyCards.Where(c => newCheckedNumbers.Contains(c.ViewData.Number));
            IEnumerable<Card> firedCardsPlayer = handPlayerCards.Where(c => newCheckedNumbers.Contains(c.ViewData.Number));

            CallbackHandler fireEnemyCallbackHandler = new CallbackHandler();
            CallbackHandler firePlayerCallbackHandler = new CallbackHandler();

            if (firedCardsEnemy.Count() > 0)
            {
                FiringCards(firedCardsEnemy, TransitFromType.HandEnemy, TransitToType.EnemyFirePool, fireEnemyCallbackHandler).ToUniTask();
            }
            else
            {
                fireEnemyCallbackHandler.Complete();
            }

            if (firedCardsPlayer.Count() > 0)
            {
                FiringCards(firedCardsPlayer, TransitFromType.HandPlayer, TransitToType.PlayerFirePool, firePlayerCallbackHandler).ToUniTask();
            }
            else
            {
                firePlayerCallbackHandler.Complete();
            }

            yield return new WaitUntil(() => fireEnemyCallbackHandler.IsComplete && firePlayerCallbackHandler.IsComplete);
        }

        private IEnumerator FiringCards(IEnumerable<Card> firedCards, TransitFromType transitFrom, TransitToType transitTo,
            CallbackHandler callbackHandler)
        {
            foreach (Card firedCard in firedCards)
            {
                _transitManager.TransitCard(firedCard, transitFrom, transitTo);
                yield return new WaitForSeconds(0.2f);
            }

            callbackHandler.Complete();
        }
    }
}