using Cards.Effects;
using System;

namespace GameFields.Effects
{
    public class JusticeBullEffect : VariantEffect
    {
        public JusticeBullEffect(VariantCardCreator variantCardCreator,
            Func<EffectType, CardEffectData, EffectDuration, Effect> effectCreator, EffectType effectType, EffectData data) :
            base(variantCardCreator, effectCreator, effectType, data)
        { }

        protected override string GetName() => nameof(JusticeBullEffect);

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("Эффект Быка Правосудия окончен");
        //}
    }
}