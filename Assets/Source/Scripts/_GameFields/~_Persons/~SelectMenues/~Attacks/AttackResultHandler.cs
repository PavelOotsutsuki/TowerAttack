using Cards;
using Cysharp.Threading.Tasks;
using GameFields.CommonAnimations;
using GameFields.DiscardPiles;
using Servers;
using Tools;

namespace GameFields.Persons.SelectMenues.Attacks
{
    public abstract class AttackResultHandler : ISelectResultHandler, ICompletable, IPersonObject
    {
        private readonly InvertCardAnimation _invertCardAnimation;
        private readonly DiscardPile _discardPile;
        private readonly LoseActions _loseActions;
        private readonly IAttackCardKeeper _attackCardKeeper;
        private readonly AttackResultHandlerData _data;
        private readonly FightProcessDBManager _fightProcessDBManager;

        private Card _currentCard;
        private bool _isComplete;
        private bool _isActive;

        public AttackResultHandler(DiscardPile discardPile, LoseActions loseActions, IAttackCardKeeper attackCardKeeper,
            AttackResultHandlerData data, FightProcessDBManager fightProcessDBManager)
        {
            _discardPile = discardPile;
            _loseActions = loseActions;
            _attackCardKeeper = attackCardKeeper;
            _data = data;
            _fightProcessDBManager = fightProcessDBManager;

            _isComplete = false;
            _isActive = false;

            _invertCardAnimation = new InvertCardAnimation(_data.InvertCardAnimationData);
        }

        public bool IsComplete => _isComplete && _isActive;

        protected virtual bool? IsPlayersAction => null;

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

        protected abstract string GetName();

        private async UniTask SettingResult(SetSelectResultData data)
        {
            await OnSettingResult(data);
            //Debug.Log($"AttackResultHandler {data.ResultType}");
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

        private async UniTask FalledAttackProcessing(CancellationTokenData data)
        {
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, null, "FALLED", GetName());

            if (_currentCard is not null)
            {
                _invertCardAnimation.Play(_currentCard, data.Token);

                await UniTask.WaitUntil(() => _invertCardAnimation.IsComplete, cancellationToken: data.Token);

                _discardPile.SeatCard(_currentCard);
            }

            _isComplete = true;
        }

        private async UniTask SuccessAttackProcessing(CancellationTokenData data)
        {
            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, IsPlayersAction, null, "SUCCESS", GetName());

            if (_currentCard is not null)
            {
                _invertCardAnimation.Play(_currentCard, data.Token);

                await UniTask.WaitUntil(() => _invertCardAnimation.IsComplete, cancellationToken: data.Token);

                _discardPile.SeatCard(_currentCard);
            }

            _loseActions.Activate(data);

            // не было, и не надо. Кнопка при победе переворачивтаься не должна
            //_isComplete = true;
        }
    }
}