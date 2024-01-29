using System.Collections;
using DG.Tweening;
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

        public ChangeSideAnimator(GameObject activeView, GameObject deactiveView, RectTransform buttonTransform, float activeViewInvertDuration, float deactiveViewInvertDuration, IEndTurnHandler endTurnHandler)
        {
            _activeView = activeView;
            _deactiveView = deactiveView;
            _buttonTransform = buttonTransform;
            _endTurnHandler = endTurnHandler;

            _activeViewInvertDuration = activeViewInvertDuration;
            _deactiveViewInvertDuration = deactiveViewInvertDuration;
            _activeViewInvertDelay = new WaitForSeconds(_activeViewInvertDuration);
            _deactiveViewInvertDelay = new WaitForSeconds(_deactiveViewInvertDuration);
        }

        public IEnumerator PlayLockButtonAnimation()
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

        public IEnumerator PlayUnlockButtonAnimation()
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

            TranslateLinear(downWay, endRotationVector, duration, scaleVector);
        }

        private void InvertDeactiveSide(float duration, float rotation)
        {
            Vector3 endRotationVector = new Vector3(rotation, 0f, 0f);
            Vector3 scaleVector = _buttonTransform.localScale;
            Vector3 downWay = _buttonTransform.position;

            TranslateSmoothly(downWay, endRotationVector, duration, scaleVector);
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

        private void TranslateLinear(Vector3 downWay, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            _buttonTransform.DOMove(downWay, duration).SetEase(Ease.Linear);
            _buttonTransform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear);
            _buttonTransform.DOScale(scaleVector, duration).SetEase(Ease.Linear);
        }

        private void TranslateSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _buttonTransform.DOMove(positon, duration);
            _buttonTransform.DORotate(rotation, duration);
            _buttonTransform.DOScale(scaleVector, duration);
        }
    }
}
