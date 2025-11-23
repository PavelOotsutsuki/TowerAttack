using System.Collections;
using Cards.Sounds;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace Cards.Animations.Fires
{
    internal class OnFireLogic : IWorkable<OnFireLogicActivateData>
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

            Activating(data).ToUniTask();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _cardFireAnimator.Deactivate();
        }

        private IEnumerator Activating(OnFireLogicActivateData data)
        {
            yield return data.Delay;

            _cardFireAnimator.Activate();

            yield return new WaitUntil(() => _cardFireAnimator.IsComplete);

            yield return new WaitForSeconds(1f);

            data.CallbackHandler.Complete();
        }
    }
}