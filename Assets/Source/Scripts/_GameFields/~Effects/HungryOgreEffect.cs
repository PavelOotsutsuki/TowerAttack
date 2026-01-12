using Cards.Effects;
using GameFields.Persons;
using System;
using UnityEngine;

namespace GameFields.Effects
{
    public class HungryOgreEffect : VariantEffect
    {
        public HungryOgreEffect(Person activePerson, VariantCardCreator variantCardCreator,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectType effectType, EffectData data) :
            base(activePerson, variantCardCreator, effectCreator, effectType, data)
        { }

        public override void End()
        {
            base.End();

            Debug.Log("Эффект Голодного Огра окончен");
        }
    }
}