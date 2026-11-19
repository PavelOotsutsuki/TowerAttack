using Cards.Effects;
using System;

namespace GameFields.Effects
{
    public class FallenGuardianEffect : VariantEffect
    {
        public FallenGuardianEffect(VariantCardCreator variantCardCreator,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectType effectType, EffectData data) :
            base(variantCardCreator, effectCreator, effectType, data)
        { }

        protected override string GetName() => nameof(FallenGuardianEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Падшего Хранителя окончен");
        //}
    }
}