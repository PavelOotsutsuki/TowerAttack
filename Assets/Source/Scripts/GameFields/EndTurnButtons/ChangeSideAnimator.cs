using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using DG.Tweening;
using UnityEngine;

namespace GameFields.EndTurnButtons
{
    public class ChangeSideAnimator
    {
        private readonly float _lockRotation = -90f;
        private readonly float _unlockRotation = 90f;

        private GameObject _activeView;
        private GameObject _deactiveView;
        private RectTransform _buttonTransform;

        private float _activeViewInvertDuration;
        private float _deactiveViewInvertDuration;
        private WaitForSeconds _activeViewInvertDelay;
        private WaitForSeconds _deactiveViewInvertDelay;

        public ChangeSideAnimator(GameObject activeView, GameObject deactiveView, RectTransform buttonTransform, float activeViewInvertDuration, float deactiveViewInvertDuration)
        {
            _activeView = activeView;
            _deactiveView = deactiveView;
            _buttonTransform = buttonTransform;

            _activeViewInvertDuration = activeViewInvertDuration;
            _deactiveViewInvertDuration = deactiveViewInvertDuration;
            _activeViewInvertDelay = new WaitForSeconds(_activeViewInvertDuration);
            _deactiveViewInvertDelay = new WaitForSeconds(_deactiveViewInvertDuration);
        }

        public IEnumerator PlayLockButtonAnimation()
        {
            if (_activeView.activeSelf == true)
            {
                InvertActiveSide(_activeViewInvertDuration, _lockRotation);
                yield return _activeViewInvertDelay;

                SetLockSide();

                InvertDeactiveSide(_deactiveViewInvertDuration, _lockRotation);
                yield return _deactiveViewInvertDelay;
            }
        }

        public IEnumerator PlayUnlockButtonAnimation()
        {
            if (_activeView.activeSelf == false)
            {
                InvertActiveSide(_activeViewInvertDuration, _unlockRotation);
                yield return _activeViewInvertDelay;

                SetUnlockSide();

                InvertDeactiveSide(_deactiveViewInvertDuration, _unlockRotation);
                yield return _deactiveViewInvertDelay;
            }
        }

        private void InvertActiveSide(float duration, float rotation)
        {
            Debug.Log("x = " + _buttonTransform.localRotation.eulerAngles.x + ". rotation = " + rotation);
            Vector3 endRotationVector = new Vector3(_buttonTransform.localRotation.eulerAngles.x + rotation, 0f, 0f);
            Vector3 scaleVector = _buttonTransform.localScale;
            Vector3 downWay = _buttonTransform.position;

            TranslateLinear(downWay, endRotationVector, duration, scaleVector);
        }

        private void InvertDeactiveSide(float duration, float rotation)
        {
            Debug.Log("x = " + _buttonTransform.localRotation.eulerAngles.x + ". rotation = " + rotation);
            Vector3 endRotationVector = new Vector3(_buttonTransform.localRotation.eulerAngles.x + rotation, 0f, 0f);
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
