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
        //private readonly float _endEffectDelay = 1f;
        private readonly int _duration;
        private readonly Card _card;
        private readonly SignalBus _bus;

        public Effect(SignalBus bus, CardEffectData data)
        {
            IsComplete = false;
            _duration = data.Duration;
            _card = data.Card;

            _bus = bus;
        }

        public int Duration => _duration;
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
            yield return OnPlaying();

            yield return new WaitForSeconds(GameSettings.DefaultEffectDelayBeforeComplete);

            IsComplete = true;
        }
    }
}