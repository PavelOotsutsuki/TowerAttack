using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Tools.CommonAnimations
{
    public class ShakeAnimation
    {
        private readonly ShakeAnimationData _data;
        private readonly Transform _targetTransform;

        public ShakeAnimation(ShakeAnimationConfig config)
        {
            _data = config.Data;
            _targetTransform = config.TargetTransform;
        }

        public ShakeAnimation()
        {
            _data = new ShakeAnimationData();

            _targetTransform = Camera.main.transform;
        }

        public void Play()
        {
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