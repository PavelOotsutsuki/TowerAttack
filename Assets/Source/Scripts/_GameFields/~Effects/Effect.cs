using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Settings;
using UnityEngine;
using Zenject;
using GameFields.Signals;
using Cards;
using GameFields.Persons.EffectHandlers;
using GameFields.Histories;
using GameFields.Persons;

namespace GameFields.Effects
{
    public abstract class Effect: ICompletable
    {
        private readonly float _endEffectDelay;
        private readonly Card _card;
        private readonly SignalBus _bus;
        private readonly EffectDuration _effectDuration;
        private readonly PersonEffectsHandlerRoot _personEffectsHandlerRoot;
        private readonly int _duration;
        private readonly HistoryRoot _historyRoot;
        private readonly IPersonObject _activePerson;

        public Effect(EffectData data, float endEffectDelay = GameSettings.DefaultEffectDelayBeforeComplete)
        {
            IsComplete = false;

            _endEffectDelay = endEffectDelay;

            _duration = data.CardEffectData.Duration;
            _card = data.CardEffectData.Card;
            _effectDuration = data.EffectDuration;
            _personEffectsHandlerRoot = data.PersonEffectsHandlerRoot;

            _bus = data.Bus;
            _historyRoot = data.HistoryRoot;
            _activePerson = data.ActivePerson;
        }

        //public int Duration => _duration;
        public bool IsComplete { get; private set; }

        public void End()
        {
            _bus.Fire(new DiscardCardsSignal(_card));
            _personEffectsHandlerRoot.EndEffect(_card);

            OnEnd();
        }

        protected virtual void OnEnd()
        {
            HistoryData historyData = new HistoryData(_activePerson, GetEndHistoryMsg(), new HistoryCardData(_card));
            _historyRoot.AddMsg(historyData);
        }

        private string GetStartHistoryMsg()
        {
            //return $"Разыграна карта: <b>{_card.Name.ToUpper()}</b>";
            return $"Разыграна карта: ";
        }

        private string GetEndHistoryMsg()
        {
            //return $"Карта ушла в бито: <b>{_card.Name.ToUpper()}</b>";
            return $"Карта ушла в бито: ";
        }

        protected void Play()
        {
            HistoryData historyData = new HistoryData(_activePerson, GetStartHistoryMsg(), new HistoryCardData(_card));
            _historyRoot.AddMsg(historyData);

            Playing().ToUniTask();
        }

        protected abstract IEnumerator OnPlaying();

        private IEnumerator Playing()
        {
            _effectDuration.SetDuration(_duration);

            yield return OnPlaying();

            if (Mathf.Approximately(_endEffectDelay, 0f) == false)
                yield return new WaitForSeconds(_endEffectDelay);

            IsComplete = true;
        }
    }
}