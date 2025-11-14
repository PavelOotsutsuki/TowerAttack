using Cards;
using Cards.Effects;
using GameFields.Persons;
using System;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class FateMistressEffect : VariantEffect
    {
        //public FateMistressEffect(Person activePerson, VariantCardCreator variantCardCreator,
        //    Func<CardEffectConfig, Action<int> , Effect> effectCreator, Action<int> callback, EffectType effectType) :
        //    base(activePerson, variantCardCreator, effectCreator, callback, effectType)
        public FateMistressEffect(Person activePerson, VariantCardCreator variantCardCreator,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectType effectType, EffectData data) :
            base(activePerson, variantCardCreator, effectCreator, effectType, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Повелительницы Судьбы окончен");
        }
    }
}