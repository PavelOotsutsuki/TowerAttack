using System.Collections;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;

namespace GameFields.EndTurnButtons
{
    public class ChangeSideAnimator
    {
        private readonly float _activeSideRotation = 90f;
        private readonly float _deactiveSideRotation = 0f;

        private GameObject _activeView;
        private GameObject _deactiveView;
        private RectTransform _buttonTransform;

        private float _activeViewInvertDuration;
        private float _deactiveViewInvertDuration;
        private WaitForSeconds _activeViewInvertDelay;
        private WaitForSeconds _deactiveViewInvertDelay;
        private IEndTurnHandler _endTurnHandler;
        private Movement _endTurnButtonMovement;

        public ChangeSideAnimator(GameObject activeView, GameObject deactiveView, RectTransform buttonTransform, float activeViewInvertDuration, float deactiveViewInvertDuration, IEndTurnHandler endTurnHandler)
        {
            _activeView = activeView;
            _deactiveView = deactiveView;
            _buttonTransform = buttonTransform;
            _endTurnHandler = endTurnHandler;
            _endTurnButtonMovement = new Movement(_buttonTransform);

            _activeViewInvertDuration = activeViewInvertDuration;
            _deactiveViewInvertDuration = deactiveViewInvertDuration;
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
            if (_activeView.activeInHierarchy == true)
            {
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
            if (_activeView.activeInHierarchy == false)
            {
                InvertActiveSide(_activeViewInvertDuration, _activeSideRotation);
                yield return _activeViewInvertDelay;

                SetUnlockSide();

                InvertDeactiveSide(_deactiveViewInvertDuration, _deactiveSideRotation);
                yield return _deactiveViewInvertDelay;
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
