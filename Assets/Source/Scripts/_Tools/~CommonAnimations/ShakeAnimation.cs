using System.Collections;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tools.CommonAnimations
{
    public class ShakeAnimation
    {
        private readonly ShakeAnimationData _data;
        private readonly Transform _targetTransform;

        private readonly ShakeAnimation _cameraShakeAnimation;

        public ShakeAnimation(ShakeAnimationConfig config, bool isIncludeCamera = true)
        {
            _data = config.Data;
            _targetTransform = config.TargetTransform;

            if (isIncludeCamera)
                _cameraShakeAnimation = new ShakeAnimation();
        }

        public ShakeAnimation()
        {
            _data = new ShakeAnimationData();

            _targetTransform = Camera.main.transform;
        }

        public void Play(CancellationToken token)
        {
            _cameraShakeAnimation?.Play(token);

            Shaking(token).Forget();
        }

        private async UniTask Shaking(CancellationToken token)
        {
            float duration = _data.Duration;
            Vector3 originalPosition = _targetTransform.position;

            //WaitForSeconds delay = new WaitForSeconds(_data.Delay);

            float x;
            float y;
            float timeLeft = Time.time;

            while ((timeLeft + duration) > Time.time)
            {
                x = Random.Range(_data.MinOffsetX, _data.MaxOffsetX);
                y = Random.Range(_data.MinOffsetY, _data.MaxOffsetY);

                _targetTransform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

                await UniTask.WaitForSeconds(_data.Delay, cancellationToken: token);
            }

            _targetTransform.position = originalPosition;
        }
    }
}