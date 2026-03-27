using Cards.Effects;
using GameFields.Persons;
using System;
using UnityEngine;

namespace GameFields.Effects
{
    public class FateMistressEffect : VariantEffect
    {
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