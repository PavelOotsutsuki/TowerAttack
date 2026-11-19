using UnityEngine;

namespace Tools
{
    public class ReadOnlyTransform
    {
        private readonly Transform _transform;

        public ReadOnlyTransform(Transform transform)
        {
            _transform = transform;
        }

        public float GetPositionX()
        {
            return _transform.position.x;
        }

        public float GetPositionY()
        {
            return _transform.position.y;
        }

        public float GetPositionZ()
        {
            return _transform.position.z;
        }

        public Vector3 GetPosition()
        {
            return _transform.position;
        }

        public Vector3 GetLocalPosition()
        {
            return _transform.localPosition;
        }

        public Vector3 GetLocalScale()
        {
            return _transform.localScale;
        }

        public Vector3 GetRotationVector()
        {
            return _transform.rotation.eulerAngles;
        }

        public void SetParent(Transform container)
        {
            _transform.SetParent(container);
        }
    }
}