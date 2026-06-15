using System;
using System.Threading;
using Cards.Effects;
using Cysharp.Threading.Tasks;
using GameFields.Histories;
using GameFields.Persons;
using GameFields.Persons.EffectHandlers;
using Zenject;

namespace GameFields.Effects
{
    public class DoubleEffect : Effect
    {
        private readonly Func<CardEffectConfigPair,EffectDuration, Effect> _effectCreator;
        private readonly CardEffectConfigPair _effectConfig;
        private readonly EffectDuration _effectDuration;
        //private readonly Action<int> _callback;

        //public DoubleEffect(Func<CardEffectConfig, Action<int>, Effect> effectCreator, CardEffectConfig effectConfig,
        //    Action<int> callback) :
        public DoubleEffect(Func<CardEffectConfigPair, EffectDuration, Effect> effectCreator, CardEffectConfigPair effectConfig,
            SignalBus bus, EffectDuration effectDuration, PersonEffectsHandlerRoot personEffectsHandlerRoot, HistoryRoot historyRoot,
            Person activePerson, CancellationToken token) : base(new EffectData(bus, effectConfig.CardEffectData, effectDuration, personEffectsHandlerRoot,
                historyRoot, activePerson, token), 0f)
        {
            _effectCreator = effectCreator;
            _effectConfig = effectConfig;
            _effectDuration = effectDuration;
            //_callback = callback;

            Play();
        }

        protected override async UniTask OnPlaying()
        {
            //Effect effect1 = _effectCreator.Invoke(_effectConfig, _callback);
            Effect effect1 = _effectCreator.Invoke(_effectConfig, _effectDuration);

            await UniTask.WaitUntil(() => effect1.IsComplete, cancellationToken: Token);

            //Effect effect2 = _effectCreator.Invoke(_effectConfig, _callback);
            Effect effect2 = _effectCreator.Invoke(_effectConfig, _effectDuration);

            await UniTask.WaitUntil(() => effect2.IsComplete,cancellationToken: Token);
        }

        //public override void End()
        //{
        //    base.End();

        //    Debug.Log("DoubleEffect completed");
        //}
    }
}