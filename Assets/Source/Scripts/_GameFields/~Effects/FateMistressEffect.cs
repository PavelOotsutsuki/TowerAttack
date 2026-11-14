using Cards.Effects;
using System;

namespace GameFields.Effects
{
    public class FateMistressEffect : VariantEffect
    {
        public FateMistressEffect(VariantCardCreator variantCardCreator,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectType effectType, EffectData data) :
            base(variantCardCreator, effectCreator, effectType, data)
        { }

        protected override string GetName() => nameof(FateMistressEffect);


        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Повелительницы Судьбы окончен");
        //}
    }
}