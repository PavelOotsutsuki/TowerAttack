using System.Collections;
using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using GameFields.Persons;
using GameFields.Persons.SelectMenues;
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
        private readonly LoseActions _loseActions;
        private readonly IAttackCardKeeper _attackCardKeeper;
        private readonly AttackResultHandlerData _data;

        private Card _currentCard;
        private bool _isComplete;
        private bool _isActive;

        public AttackResultHandler(DiscardPile discardPile, LoseActions loseActions, IAttackCardKeeper attackCardKeeper,
            AttackResultHandlerData data)
        {
            _discardPile = discardPile;
            _loseActions = loseActions;
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

            SettingResult(data).Forget();
        }

        protected virtual UniTask OnSettingResult(SetSelectResultData data)
        {
            _currentCard = _attackCardKeeper.SeizeAttackingCard;
            return UniTask.CompletedTask;
        }

        private async UniTask SettingResult(SetSelectResultData data)
        {
            await OnSettingResult(data);
            Debug.Log($"AttackResultHandler {data.ResultType}");
            switch (data.ResultType)
            {
                case ResultType.Success:
                    await SuccessAttackProcessing(data);
                    break;
                case ResultType.Falled:
                    await FalledAttackProcessing(data);
                    break;
                default:
                    throw new System.Exception("Неизвестный ResultType");
            }

            await UniTask.Delay(1000, cancellationToken: data.Token);
            _isActive = false;
        }

        private async UniTask FalledAttackProcessing(CancellationTokenData tokenData)
        {
            if (_currentCard is not null)
            {
                _invertCardAnimation.Play(_currentCard, tokenData.Token);

                await UniTask.WaitUntil(() => _invertCardAnimation.IsComplete, cancellationToken: tokenData.Token);

                _discardPile.SeatCard(_currentCard);
            }

            _isComplete = true;
        }

        private async UniTask SuccessAttackProcessing(CancellationTokenData tokenData)
        {
            if (_currentCard is not null)
            {
                _invertCardAnimation.Play(_currentCard, tokenData.Token);

                await UniTask.WaitUntil(() => _invertCardAnimation.IsComplete, cancellationToken: tokenData.Token);

                _discardPile.SeatCard(_currentCard);
            }

            _loseActions.Activate(tokenData);

            // не было, и не надо. Кнопка при победе переворачивтаься не должна
            //_isComplete = true;
        }
    }
}