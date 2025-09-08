using Cards;
using GameFields.Persons.Commons;
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
            Func<EffectType, CardEffectData, Effect> effectCreator, EffectType effectType, SignalBus bus, CardEffectData data) :
            base(activePerson, variantCardCreator, effectCreator, effectType, bus, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Повелительницы Судьбы окончен");
        }
    }
}