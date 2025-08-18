using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.Towers;
using GameFields.Signals;
using Tools;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public abstract class AttackResultHandler : ISelectResultHandler, ICompletable, IPersonObject
    {
        private readonly InvertCardAnimation _invertCardAnimation;
        private readonly DiscardPile _discardPile;
        private readonly SignalBus _bus;
        private readonly IBoomTower _tower;
        private readonly IAttackCardKeeper _attackCardKeeper;
        private readonly AttackResultHandlerData _data;

        private Card _currentCard;
        private bool _isComplete;
        private bool _isActive;

        public AttackResultHandler(DiscardPile discardPile, SignalBus bus, IBoomTower tower, IAttackCardKeeper attackCardKeeper,
            AttackResultHandlerData data)
        {
            _discardPile = discardPile;
            _bus = bus;
            _tower = tower;
            _attackCardKeeper = attackCardKeeper;
            _data = data;

            _isComplete = false;
            _isActive = false;

            _invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
        }

        public bool IsComplete => _isComplete && _isActive;

        public void SetResult(SetSelectResultData data)
        {
            _isComplete = false;
            _isActive = true;

            SettingResult(data).ToUniTask();
        }

        protected virtual IEnumerator OnSettingResult(SetSelectResultData data)
        {
            _currentCard = _attackCardKeeper.SeizeAttackingCard;
            yield break;
        }

        private IEnumerator SettingResult(SetSelectResultData data)
        {
            yield return OnSettingResult(data);

            switch (data.ResultType)
            {
                case ResultType.Success:
                    yield return SuccessAttackProcessing();
                    break;
                case ResultType.Falled:
                    yield return FalledAttackProcessing();
                    break;
                default:
                    throw new System.Exception("Неизвестный ResultType");
            }

            yield return new WaitForSeconds(1f);
            _isActive = false;
        }

        private IEnumerator FalledAttackProcessing()
        {
            if (_currentCard is not null)
            {
                _invertCardAnimation.Play(_currentCard);

                yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

                _discardPile.SeatCard(_currentCard);
            }

            _isComplete = true;
        }

        private IEnumerator SuccessAttackProcessing()
        {
            if (_currentCard is not null)
            {
                _invertCardAnimation.Play(_currentCard);

                yield return new WaitUntil(() => _invertCardAnimation.IsComplete);

                _discardPile.SeatCard(_currentCard);
            }

            _tower.Boom();

            yield return new WaitForSeconds(_data.DelayBeforeStartingEndFightActions);

            _bus.Fire(new PersonWinSignal(this));

            // не было, и не надо. Кнопка при победе переворачивтаься не должна
            //_isComplete = true;
        }
    }
}