using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Persons;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class DumbMonkEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly CardLocationViewRoot _viewRoot;
        private readonly CardTransitManager _transitManager;

        private bool _isEffectComplete;

        public DumbMonkEffect(Person activePerson, CardLocationViewRoot viewRoot, CardTransitManager transitManager,
            EffectData data) : base(data)
        {
            _activePerson = activePerson;
            _viewRoot = viewRoot;
            _transitManager = transitManager;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Глупого Монаха закончен");
        }

        protected override IEnumerator OnPlaying()
        {
            _isEffectComplete = false;

            ViewType viewType = _activePerson is Player ? ViewType.HandPlayer : ViewType.HandAI;
            IReadOnlyList<Card> cards = _viewRoot.GetAllCards(viewType).ToList();

            if (cards.Count <= 0)
            {
                EffectComplete();
                yield break;
            }

            TransitFromType transitFromType;
            TransitToType transitToType;

            if (viewType == ViewType.HandPlayer)
            {
                transitFromType = TransitFromType.HandPlayer;
                transitToType = TransitToType.PlayerFirePool;
            }
            else
            {
                transitFromType = TransitFromType.HandEnemy;
                transitToType = TransitToType.EnemyFirePool;
            }

            _transitManager.TransitCard(cards[0], transitFromType, transitToType, EffectComplete);
            //_activePerson.ActivateFateInevitability(_duration);
            yield return new WaitUntil(() => _isEffectComplete);
        }

        private void EffectComplete()
        {
            _isEffectComplete = true;
        }
    }
}