using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Tools.Utils.Movements;

namespace GameFields.EndTurnButtons
{
    public class ChangeSideAnimator
    {
        private readonly ChangeSideAnimatorData _data;
        private readonly Button _button;
        private readonly WaitForSeconds _activeViewInvertDelay;
        private readonly WaitForSeconds _deactiveViewInvertDelay;
        private readonly Movement _endTurnButtonMovement;

        private bool _isAnimationInWork;

        public ChangeSideAnimator(ChangeSideAnimatorData data, Button button)
        {
            _data = data;
            _button = button;

            _endTurnButtonMovement = new Movement(_data.ButtonTransform);

            _isAnimationInWork = false;
            IsActiveSide = false;

            _activeViewInvertDelay = new WaitForSeconds(_data.ActiveViewInvertDuration);
            _deactiveViewInvertDelay = new WaitForSeconds(_data.DeactiveViewInvertDuration);
        }

        public bool IsActiveSide { get; private set; }

        public void PlayLockButtonAnimation()
        {
            PlayingLockButtonAnimation().ToUniTask();
        }

        public void PlayUnlockButtonAnimation()
        {
            PlayingUnlockButtonAnimation().ToUniTask();
        }

        private IEnumerator PlayingLockButtonAnimation()
        {
            IsActiveSide = false;

            yield return new WaitWhile(() => _isAnimationInWork);

            if (_button.interactable)
            {
                _button.interactable = false;

                _isAnimationInWork = true;

                InvertActiveSide(_data.ActiveViewInvertDuration, _data.ActiveSideRotation);
                yield return _activeViewInvertDelay;

                SetLockSide();

                InvertDeactiveSide(_data.DeactiveViewInvertDuration, _data.DeactiveSideRotation);
                yield return _deactiveViewInvertDelay;

                _isAnimationInWork = false;
            }
        }

        private IEnumerator PlayingUnlockButtonAnimation()
        {
            IsActiveSide = true;

            yield return new WaitWhile(() => _isAnimationInWork);

            if (_button.interactable == false)
            {
                _isAnimationInWork = true;

                InvertActiveSide(_data.ActiveViewInvertDuration, _data.ActiveSideRotation);
                yield return _activeViewInvertDelay;

                SetUnlockSide();

                InvertDeactiveSide(_data.DeactiveViewInvertDuration, _data.DeactiveSideRotation);
                yield return _deactiveViewInvertDelay;

                _button.interactable = true;

                _isAnimationInWork = false;
            }
        }

        private void InvertActiveSide(float duration, float rotation)
        {
            Vector3 endRotationVector = new Vector3(rotation, 0f, 0f);
            Vector3 scaleVector = _data.ButtonTransform.localScale;
            Vector3 downWay = _data.ButtonTransform.position;

            _endTurnButtonMovement.MoveLinear(downWay, endRotationVector, duration, scaleVector);
        }

        private void InvertDeactiveSide(float duration, float rotation)
        {
            Vector3 endRotationVector = new Vector3(rotation, 0f, 0f);
            Vector3 scaleVector = _data.ButtonTransform.localScale;
            Vector3 downWay = _data.ButtonTransform.position;

            _endTurnButtonMovement.MoveSmoothly(downWay, endRotationVector, duration, scaleVector);
        }

        private void SetLockSide()
        {
            _data.ActiveView.SetActive(false);
            _data.DeactiveView.SetActive(true);
        }

        private void SetUnlockSide()
        {
            _data.ActiveView.SetActive(true);
            _data.DeactiveView.SetActive(false);
        }
    }
}