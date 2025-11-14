using System.Collections;
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

        public void Play()
        {
            _cameraShakeAnimation?.Play();

            Shaking().ToUniTask();
        }

        private IEnumerator Shaking()
        {
            float duration = _data.Duration;
            Vector3 originalPosition = _targetTransform.position;

            WaitForSeconds delay = new WaitForSeconds(_data.Delay);

            float x;
            float y;
            float timeLeft = Time.time;

            while ((timeLeft + duration) > Time.time)
            {
                x = Random.Range(_data.MinOffsetX, _data.MaxOffsetX);
                y = Random.Range(_data.MinOffsetY, _data.MaxOffsetY);

                _targetTransform.position = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);

                yield return delay;
            }

            _targetTransform.position = originalPosition;
        }
    }
}