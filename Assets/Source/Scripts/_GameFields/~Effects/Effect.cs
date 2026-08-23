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
using System.Threading;
using Servers;
using Cards.Effects;

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
        private readonly FightProcessDBManager _fightProcessDBManager;
        private readonly EffectType _playedEffectType;

        protected CancellationToken Token;

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
            _fightProcessDBManager = data.FightProcessDBManager;
            _playedEffectType = data.PlayedEffectType;
            Token = data.FightToken;
        }

        //public int Duration => _duration;
        public bool IsComplete { get; private set; }

        protected bool? IsPlayersAction => _activePerson is IPlayerObject ? true : _activePerson is IEnemyAIObject ? false : null;

        public void End()
        {
            _bus.Fire(new DiscardCardsSignal(_card, Token));
            _personEffectsHandlerRoot.EndEffect(_card);

            OnEnd();
        }

        protected virtual void OnEnd()
        {
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, GetType().Name, "END", "EFFECT");
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, _card.EffectConfig.Type.ToString(), "END", "ORIGINAL_EFFECTTYPE");
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, _playedEffectType.ToString(), "END", "PLAYED_EFFECTTYPE");
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
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, GetName(), "START", "EFFECT");
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, _card.EffectConfig.Type.ToString(), "START", "ORIGINAL_EFFECTTYPE");
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, _playedEffectType.ToString(), "START", "PLAYED_EFFECTTYPE");
            HistoryData historyData = new HistoryData(_activePerson, GetStartHistoryMsg(), new HistoryCardData(_card));
            _historyRoot.AddMsg(historyData);

            Playing().Forget();
        }

        protected abstract UniTask OnPlaying();
        protected abstract string GetName();

        private async UniTask Playing()
        {
            _effectDuration.SetDuration(_duration);

            await OnPlaying();

            if (Mathf.Approximately(_endEffectDelay, 0f) == false)
                await UniTask.WaitForSeconds(_endEffectDelay, cancellationToken: Token);

            IsComplete = true;
        }
    }
}