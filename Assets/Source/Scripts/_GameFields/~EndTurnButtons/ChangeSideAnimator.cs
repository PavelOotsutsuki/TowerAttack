using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Tools.Utils.Movements;
using System.Threading;

namespace GameFields.EndTurnButtons
{
    public class ChangeSideAnimator
    {
        private readonly ChangeSideAnimatorData _data;
        private readonly Button _button;
        private readonly CancellationToken _fightToken;
        private readonly Movement _endTurnButtonMovement;

        private bool _isAnimationInWork;

        public ChangeSideAnimator(ChangeSideAnimatorData data, Button button, CancellationToken fightToken)
        {
            _data = data;
            _button = button;
            _fightToken = fightToken;

            _endTurnButtonMovement = new Movement(_data.ButtonTransform);

            _isAnimationInWork = false;
            IsActiveSide = false;
        }

        public bool IsActiveSide { get; private set; }

        private float ActiveViewInvertDelay => _data.ActiveViewInvertDuration;
        private float DeactiveViewInvertDelay => _data.DeactiveViewInvertDuration;

        public void PlayLockButtonAnimation()
        {
            PlayingLockButtonAnimation(_fightToken).Forget();
        }

        public void PlayUnlockButtonAnimation()
        {
            PlayingUnlockButtonAnimation(_fightToken).Forget();
        }

        private async UniTask PlayingLockButtonAnimation(CancellationToken token)
        {            
            IsActiveSide = false;

            await UniTask.WaitWhile(() => _isAnimationInWork, cancellationToken: token);

            if (_button.interactable)
            {
                _button.interactable = false;

                _isAnimationInWork = true;

                InvertActiveSide(ActiveViewInvertDelay, _data.ActiveSideRotation);
                await UniTask.WaitForSeconds(ActiveViewInvertDelay, cancellationToken: token);

                SetLockSide();

                InvertDeactiveSide(DeactiveViewInvertDelay, _data.DeactiveSideRotation);
                await UniTask.WaitForSeconds(DeactiveViewInvertDelay, cancellationToken: token);

                _isAnimationInWork = false;
            }
        }

        private async UniTask PlayingUnlockButtonAnimation(CancellationToken token)
        {
            IsActiveSide = true;

            await UniTask.WaitWhile(() => _isAnimationInWork, cancellationToken: token);

            if (_button.interactable == false)
            {
                _isAnimationInWork = true;

                InvertActiveSide(ActiveViewInvertDelay, _data.ActiveSideRotation);
                await UniTask.WaitForSeconds(ActiveViewInvertDelay, cancellationToken: token);

                SetUnlockSide();

                InvertDeactiveSide(DeactiveViewInvertDelay, _data.DeactiveSideRotation);
                await UniTask.WaitForSeconds(DeactiveViewInvertDelay, cancellationToken: token);

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