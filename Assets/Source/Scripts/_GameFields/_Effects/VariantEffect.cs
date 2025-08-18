using UnityEngine;
using Cards;
using GameFields.Persons.Commons;
using System.Collections;
using GameFields.Persons.Discovers;
using System.Collections.Generic;
using System;

namespace GameFields.Effects
{
    public abstract class VariantEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly VariantCardCreator _variantCardCreator;
        private readonly Func<CardEffectConfig, Action<int>, Effect> _effectCreator;
        private readonly Action<int> _callback;
        private readonly EffectType _effectType;

        public VariantEffect(Person activePerson, VariantCardCreator variantCardCreator,
            Func<CardEffectConfig, Action<int>, Effect> effectCreator, Action<int> callback,
            EffectType effectType) : base()
        {
            _activePerson = activePerson;
            _variantCardCreator = variantCardCreator;
            _effectCreator = effectCreator;
            _callback = callback;
            _effectType = effectType;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            IReadOnlyList<VariantCard> variantCards = _variantCardCreator.CreateByEffect(_effectType);

            DiscoverResult discoverResult = new DiscoverResult();
            _activePerson.DiscoverCards(variantCards, "Выберите эффект", discoverResult);

            yield return new WaitUntil(() => discoverResult.IsComplete);

            VariantCard variantCard = (VariantCard)discoverResult.Result;
            CardEffectConfig effectConfig = variantCard.EffectConfig;

            Effect realEffect = _effectCreator.Invoke(effectConfig, _callback);

            yield return new WaitUntil(() => realEffect.IsComplete);
        }
    }
}