using System;
using System.Collections;
using Cards;
using UnityEngine;

namespace GameFields.Effects
{
    public class DoubleEffect : Effect
    {
        private readonly Func<CardEffectConfig, Effect> _effectCreator;
        private readonly CardEffectConfig _effectConfig;

        public DoubleEffect(Func<CardEffectConfig, Effect> effectCreator, CardEffectConfig effectConfig) :
            base(effectConfig.Duration)
        {
            _effectCreator = effectCreator;
            _effectConfig = effectConfig;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            Effect effect1 = _effectCreator.Invoke(_effectConfig);

            yield return new WaitUntil(() => effect1.IsComplete);

            Effect effect2 = _effectCreator.Invoke(_effectConfig);

            yield return new WaitUntil(() => effect2.IsComplete);
        }

        public override void End()
        {
            Debug.Log("DoubleEffect completed");
        }
    }
}