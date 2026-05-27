using Cards.Effects;
using GameFields.Persons;
using System;
using UnityEngine;

namespace GameFields.Effects
{
    public class StrongOgreEffect : VariantEffect
    {
        public StrongOgreEffect(VariantCardCreator variantCardCreator,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectType effectType, EffectData data) :
            base(variantCardCreator, effectCreator, effectType, data)
        { }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Сильного Огра окончен");
        //}
    }
}