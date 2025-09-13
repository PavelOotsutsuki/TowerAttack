using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Settings;
using UnityEngine;
using Zenject;
using GameFields.Signals;
using Cards;

namespace GameFields.Effects
{
    public abstract class Effect: ICompletable
    {
        private readonly float _endEffectDelay;
        private readonly Card _card;
        private readonly SignalBus _bus;
        private readonly EffectDuration _effectDuration;

        protected readonly int Duration;

        public Effect(EffectData data, float endEffectDelay = GameSettings.DefaultEffectDelayBeforeComplete)
        {
            IsComplete = false;

            _endEffectDelay = endEffectDelay;

            Duration = data.CardEffectData.Duration;
            _card = data.CardEffectData.Card;
            _effectDuration = data.EffectDuration;

            _bus = data.Bus;
        }

        //public int Duration => _duration;
        public bool IsComplete { get; private set; }

        public virtual void End()
        {
            _bus.Fire(new DiscardCardsSignal(_card));
        }

        protected void Play()
        {
            Playing().ToUniTask();
        }

        protected abstract IEnumerator OnPlaying();

        private IEnumerator Playing()
        {
            _effectDuration.SetDuration(Duration);

            yield return OnPlaying();

            if (Mathf.Approximately(_endEffectDelay, 0f) == false)
                yield return new WaitForSeconds(_endEffectDelay);

            IsComplete = true;
        }
    }
}