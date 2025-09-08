using System;
using System.Collections;
using Cards;
using UnityEngine;
using Zenject;

namespace GameFields.Effects
{
    public class DoubleEffect : Effect
    {
        private readonly Func<CardEffectConfigPair, Effect> _effectCreator;
        private readonly CardEffectConfigPair _effectConfig;
        //private readonly Action<int> _callback;

        //public DoubleEffect(Func<CardEffectConfig, Action<int>, Effect> effectCreator, CardEffectConfig effectConfig,
        //    Action<int> callback) :
        public DoubleEffect(Func<CardEffectConfigPair, Effect> effectCreator, CardEffectConfigPair effectConfig,
            SignalBus bus) :
            base(bus, effectConfig.CardEffectData)
        {
            _effectCreator = effectCreator;
            _effectConfig = effectConfig;
            //_callback = callback;

            Play();
        }

        protected override IEnumerator OnPlaying()
        {
            //Effect effect1 = _effectCreator.Invoke(_effectConfig, _callback);
            Effect effect1 = _effectCreator.Invoke(_effectConfig);

            yield return new WaitUntil(() => effect1.IsComplete);

            //Effect effect2 = _effectCreator.Invoke(_effectConfig, _callback);
            Effect effect2 = _effectCreator.Invoke(_effectConfig);

            yield return new WaitUntil(() => effect2.IsComplete);
        }

        public override void End()
        {
            base.End();

            Debug.Log("DoubleEffect completed");
        }
    }
}