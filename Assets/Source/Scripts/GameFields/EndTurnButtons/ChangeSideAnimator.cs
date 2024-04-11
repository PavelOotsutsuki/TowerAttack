using System.Collections;
using Cysharp.Threading.Tasks;
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
        private IEndTurnHandler _endTurnHandler;
        private Movement _endTurnButtonMovement;
        private Button _button;

        public void Init(Button button, IEndTurnHandler endTurnHandler)
        {
            _endTurnHandler = endTurnHandler;
            _endTurnButtonMovement = new Movement(_buttonTransform);
            _button = button;

            _activeViewInvertDelay = new WaitForSeconds(_activeViewInvertDuration);
            _deactiveViewInvertDelay = new WaitForSeconds(_deactiveViewInvertDuration);
        }

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
            if (_button.interactable)
            {
                _button.interactable = false;

                InvertActiveSide(_activeViewInvertDuration, _activeSideRotation);
                yield return _activeViewInvertDelay;

                SetLockSide();

                InvertDeactiveSide(_deactiveViewInvertDuration, _deactiveSideRotation);
                yield return _deactiveViewInvertDelay;

                _endTurnHandler.OnEndTurn();
            }
        }

        private IEnumerator PlayingUnlockButtonAnimation()
        {
            if (_button.interactable == false)
            {
                InvertActiveSide(_activeViewInvertDuration, _activeSideRotation);
                yield return _activeViewInvertDelay;

                SetUnlockSide();

                InvertDeactiveSide(_deactiveViewInvertDuration, _deactiveSideRotation);
                yield return _deactiveViewInvertDelay;

                _button.interactable = true;
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
