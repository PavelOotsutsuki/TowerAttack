using Cards;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Commons;
using UnityEngine;
using GameFields.Persons.Discovers;

namespace GameFields.Effects
{
    public class LeftEyedSisterEffect : Effect
    {
        private const int CountDicoverFromHand = 3;

        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        public LeftEyedSisterEffect(Person activePerson, CardLocationViewRoot viewRoot, CardTransitManager transitManager) : base()
        {
            _activePerson = activePerson;

            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        public override void End()
        {
            Debug.Log("Эффект Левоглазой сестры закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            ViewType viewType = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;

            //_activePerson

            for (int i = CountDicoverFromHand; i > 0; i--)
            {
                if (_viewRoot.TryView(out IReadOnlyList<Card> cards, i, viewType))
                {
                    DiscoverResult discoverResult = new DiscoverResult();
                    _activePerson.DiscoverCards(cards, "Выберите новую карту в замок", discoverResult);

                    yield return new WaitUntil(() => discoverResult.IsComplete);

                    _transitManager.TryExchangeTower((Card)discoverResult.Result, _activePerson, TowerTransitType.Hand);
                    yield break;
                }
            }

            yield break;
        }
    }
}