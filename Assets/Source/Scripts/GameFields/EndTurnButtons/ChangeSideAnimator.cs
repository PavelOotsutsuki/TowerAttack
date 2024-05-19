using System.Collections;
using Tools;
using UnityEngine;
using System;
using UnityEngine.UI;

namespace GameFields.EndTurnButtons
{
    [Serializable]
    public class ChangeSideAnimator
    {
        private readonly float _activeSideRotation = 90f;
        private readonly float _deactiveSideRotation = 0f;

        [SerializeField] private GameObject _activeView;
        [SerializeField] private GameObject _deactiveView;
        [SerializeField] private RectTransform _buttonTransform;
        [SerializeField] private float _activeViewInvertDuration = 0.2f;
        [SerializeField] private float _deactiveViewInvertDuration = 0.2f;

        private WaitForSeconds _activeViewInvertDelay;
        private WaitForSeconds _deactiveViewInvertDelay;
        private Movement _endTurnButtonMovement;
        private Button _button;
        private bool _isAnimationInWork;

        public void Init(Button button)
        {
            _endTurnButtonMovement = new Movement(_buttonTransform);
            _button = button;
            _isAnimationInWork = false;

            _activeViewInvertDelay = new WaitForSeconds(_activeViewInvertDuration);
            _deactiveViewInvertDelay = new WaitForSeconds(_deactiveViewInvertDuration);
        }

        public void PlayLockButtonAnimation()
        {
            _button.StartCoroutine(PlayingLockButtonAnimation());
        }

        public void PlayUnlockButtonAnimation()
        {
            _button.StartCoroutine(PlayingUnlockButtonAnimation());
        }

        private IEnumerator PlayingLockButtonAnimation()
        {
            yield return new WaitWhile(() => _isAnimationInWork);

            if (_button.interactable)
            {
                _button.interactable = false;

                _isAnimationInWork = true;

                InvertActiveSide(_activeViewInvertDuration, _activeSideRotation);
                yield return _activeViewInvertDelay;

                SetLockSide();

                InvertDeactiveSide(_deactiveViewInvertDuration, _deactiveSideRotation);
                yield return _deactiveViewInvertDelay;

                _isAnimationInWork = false;
            }
        }

        private IEnumerator PlayingUnlockButtonAnimation()
        {
            yield return new WaitWhile(() => _isAnimationInWork);

            if (_button.interactable == false)
            {
                _isAnimationInWork = true;

                InvertActiveSide(_activeViewInvertDuration, _activeSideRotation);
                yield return _activeViewInvertDelay;

                SetUnlockSide();

                InvertDeactiveSide(_deactiveViewInvertDuration, _deactiveSideRotation);
                yield return _deactiveViewInvertDelay;

                _button.interactable = true;

                _isAnimationInWork = false;
            }
        }

        private void InvertActiveSide(float duration, float rotation)
        {
            Vector3 endRotationVector = new Vector3(rotation, 0f, 0f);
            Vector3 scaleVector = _buttonTransform.localScale;
            Vector3 downWay = _buttonTransform.position;

            _endTurnButtonMovement.MoveLinear(downWay, endRotationVector, duration, scaleVector);
        }

        private void InvertDeactiveSide(float duration, float rotation)
        {
            Vector3 endRotationVector = new Vector3(rotation, 0f, 0f);
            Vector3 scaleVector = _buttonTransform.localScale;
            Vector3 downWay = _buttonTransform.position;

            _endTurnButtonMovement.MoveSmoothly(downWay, endRotationVector, duration, scaleVector);
        }

        private void SetLockSide()
        {
            _activeView.SetActive(false);
            _deactiveView.SetActive(true);
        }

        private void SetUnlockSide()
        {
            _activeView.SetActive(true);
            _deactiveView.SetActive(false);
        }
    }
}
