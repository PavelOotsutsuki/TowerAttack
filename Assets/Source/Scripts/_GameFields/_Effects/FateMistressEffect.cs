using UnityEngine;
using Cards;
using GameFields.Persons.Commons;
using System.Collections;
using GameFields.Persons.DrawCards;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Discovers;
using System.Collections.Generic;
using System;

namespace GameFields.Effects
{
    public class FateMistressEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly VariantCardCreator _variantCardCreator;
        private readonly Func<CardEffectConfig, Action<int>, Effect> _effectCreator;
        private readonly Action<int> _callback;

        public FateMistressEffect(Person activePerson, VariantCardCreator variantCardCreator,
            Func<CardEffectConfig, Action<int> , Effect> effectCreator, Action<int> callback) : base()
        {
            _activePerson = activePerson;
            _variantCardCreator = variantCardCreator;
            _effectCreator = effectCreator;
            _callback = callback;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            IReadOnlyList<VariantCard> variantCards = _variantCardCreator.CreateByEffect(EffectType.FateMistress);

            DiscoverResult discoverResult = new DiscoverResult();
            _activePerson.DiscoverCards(variantCards, "Выберите эффект", discoverResult);

            yield return new WaitUntil(() => discoverResult.IsComplete);

            VariantCard variantCard = (VariantCard)discoverResult.Result;
            CardEffectConfig effectConfig = variantCard.EffectConfig;

            Effect realEffect = _effectCreator.Invoke(effectConfig, _callback);

            yield return new WaitUntil(() => realEffect.IsComplete);
        }

        public override void End()
        {
            //Debug.Log("End patriarch corall effect");
        }
    }
}