using UnityEngine;

namespace Tools
{
    public class ReadOnlyTransform
    {
        private Transform _transform;

        public ReadOnlyTransform(Transform transform)
        {
            _transform = transform;
        }

        public float GetPositionX()
        {
            return _transform.position.x;
        }
    }
}