using UnityEngine;
using Cards;
using GameFields.Persons;
using System.Collections;
using GameFields.Persons.Discovers;
using System.Collections.Generic;
using System;
using Zenject;
using Cards.Effects;
using Cards.Views;

namespace GameFields.Effects
{
    public abstract class VariantEffect : Effect
    {
        private readonly Person _activePerson;
        private readonly VariantCardCreator _variantCardCreator;
        private readonly Func<EffectType, CardEffectData, EffectDuration, Effect> _effectCreator;
        //private readonly Action<int> _callback;
        private readonly EffectType _effectType;
        private readonly CardEffectData _data;
        private readonly EffectDuration _effectDuration;

        //public VariantEffect(Person activePerson, VariantCardCreator variantCardCreator,
        //    Func<CardEffectConfig, Action<int>, Effect> effectCreator, Action<int> callback,
        //    EffectType effectType) : base()
        public VariantEffect(Person activePerson, VariantCardCreator variantCardCreator,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectType effectType, EffectData data)
            : base(data, 0f)
        {
            _activePerson = activePerson;
            _variantCardCreator = variantCardCreator;
            _effectCreator = effectCreator;
            _data = data.CardEffectData;
            _effectDuration = data.EffectDuration;
            //_callback = callback;
            _effectType = effectType;

            Play();
        }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект ВАРИАНТ окончен");
        }

        protected override IEnumerator OnPlaying()
        {
            IReadOnlyList<VariantCard> variantCards = _variantCardCreator.CreateByEffect(_effectType);

            DiscoverResult discoverResult = new DiscoverResult();
            _activePerson.DiscoverCards(variantCards, "Выберите эффект", discoverResult);

            yield return new WaitUntil(() => discoverResult.IsComplete);

            VariantCard variantCard = (VariantCard)discoverResult.Result;
            CardEffectConfig effectConfig = variantCard.EffectConfig;

            //Effect realEffect = _effectCreator.Invoke(effectConfig, _callback);
            Effect realEffect = _effectCreator.Invoke(effectConfig.Type, new CardEffectData(_data.Card, effectConfig.Duration, null),
                _effectDuration);

            foreach (VariantCard variant in variantCards)
            {
                variant.Destroy();
            }

            yield return new WaitUntil(() => realEffect.IsComplete);
        }
    }
}