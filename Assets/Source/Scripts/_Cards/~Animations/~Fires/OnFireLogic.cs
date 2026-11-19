using Cards.Sounds;
using Cysharp.Threading.Tasks;
using Tools;

namespace Cards.Animations.Fires
{
    internal class OnFireLogic : IWorkable<OnFireLogicActivateData, CancellationTokenData>
    {
        private readonly CardFireAnimator _cardFireAnimator;
        private readonly CardSoundRoot _cardSoundRoot;
        private readonly IFireSoundKeeper _fireSoundKeeper;

        public OnFireLogic(CardFireAnimator cardFireAnimator, CardSoundRoot cardSoundRoot, CardSoundLogic cardSoundLogic)
        {
            _cardFireAnimator = cardFireAnimator;
            _cardSoundRoot = cardSoundRoot;

            if (cardSoundLogic is IFireSoundKeeper fireSoundKeeper)
            {
                _fireSoundKeeper = fireSoundKeeper;
            }
            else
            {
                _fireSoundKeeper = null;
            }
        }

        public bool? IsActive { get; protected set; } = null;

        public void Activate(OnFireLogicActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            if (_fireSoundKeeper != null)
                _cardSoundRoot.Play(_fireSoundKeeper.FireSound);

            Activating(data).Forget();
        }

        public void Deactivate(CancellationTokenData tokenData)
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _cardFireAnimator.Deactivate(tokenData);
        }

        private async UniTask Activating(OnFireLogicActivateData data)
        {
            await UniTask.WaitForSeconds(data.Delay, cancellationToken: data.Token); 

            _cardFireAnimator.Activate(data);

            await UniTask.WaitUntil(() => _cardFireAnimator.IsComplete, cancellationToken: data.Token);
            await UniTask.WaitForSeconds(1f, cancellationToken: data.Token);

            data.CallbackHandler.Complete();
        }
    }
}