using System;
using System.Collections;
using Cards;
using UnityEngine;

namespace GameFields.Effects
{
    public class DoubleEffect : Effect
    {
        private readonly Func<CardEffectConfig, Action<int>, Effect> _effectCreator;
        private readonly CardEffectConfig _effectConfig;
        private readonly Action<int> _callback;

        public DoubleEffect(Func<CardEffectConfig, Action<int>, Effect> effectCreator, CardEffectConfig effectConfig,
            Action<int> callback) :
            base(effectConfig.Duration)
        {
            _effectCreator = effectCreator;
            _effectConfig = effectConfig;
            _callback = callback;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            Effect effect1 = _effectCreator.Invoke(_effectConfig, _callback);

            yield return new WaitUntil(() => effect1.IsComplete);

            Effect effect2 = _effectCreator.Invoke(_effectConfig, _callback);

            yield return new WaitUntil(() => effect2.IsComplete);
        }

        public override void End()
        {
            Debug.Log("DoubleEffect completed");
        }
    }
}