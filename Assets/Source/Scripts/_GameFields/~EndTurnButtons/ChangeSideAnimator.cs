using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Tools.Utils.Movements;
using Tools.Utils.Screens;

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
            //Vector3 downWay = new Vector2(_data.ButtonTransform.position.x * ScreenView.GetFactorX(), _data.ButtonTransform.position.y * ScreenView.GetFactorY());
            //Vector3 downWay = new Vector2(_data.ButtonTransform.position.x, _data.ButtonTransform.position.y);
            Vector3 downWay = _data.ButtonTransform.localPosition;

            //Debug.Log($"downWay = {downWay}, _data.ButtonTransform.localPosition = {_data.ButtonTransform.localPosition}, _data.ButtonTransform.position = {_data.ButtonTransform.position}" +
            //    $", _data.ButtonTransform.rotation = {_data.ButtonTransform.rotation}, _data.ButtonTransform.localScale = {_data.ButtonTransform.localScale}");
            _endTurnButtonMovement.MoveLocalLinear(downWay, endRotationVector, duration, scaleVector);
            //_endTurnButtonMovement.MoveLocalLinear(_startPosition, endRotationVector, duration, scaleVector);
        }

        private void InvertDeactiveSide(float duration, float rotation)
        {
            Vector3 endRotationVector = new Vector3(rotation, 0f, 0f);
            Vector3 scaleVector = _data.ButtonTransform.localScale;
            //Vector3 downWay = new Vector2(_data.ButtonTransform.position.x * ScreenView.GetFactorX(), _data.ButtonTransform.position.y * ScreenView.GetFactorY());
            //Vector3 downWay = new Vector2(_data.ButtonTransform.position.x, _data.ButtonTransform.position.y);
            Vector3 downWay = _data.ButtonTransform.localPosition;

    //        Debug.Log($"downWay = {downWay}, _data.ButtonTransform.localPosition = {_data.ButtonTransform.localPosition}, _data.ButtonTransform.position = {_data.ButtonTransform.position}" +
    //$", _data.ButtonTransform.rotation = {_data.ButtonTransform.rotation}, _data.ButtonTransform.localScale = {_data.ButtonTransform.localScale}");
            _endTurnButtonMovement.MoveLocalSmoothly(downWay, endRotationVector, duration, scaleVector);
            //_endTurnButtonMovement.MoveLocalSmoothly(_startPosition, endRotationVector, duration, scaleVector);
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