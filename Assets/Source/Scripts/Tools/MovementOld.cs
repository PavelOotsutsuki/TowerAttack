using UnityEngine;
using DG.Tweening;

namespace Tools
{
    public class MovementOld
    {
        private Transform _transform;

        public MovementOld(Transform transform)
        {
            _transform = transform;
        }

        public void MoveLocalInstantly(Vector2 position, Vector3 rotation)
        {
            //_transform.localPosition = position;
            //_transform.localRotation = Quaternion.Euler(rotation);
            //if (_transform.gameObject.name.ToString().IndexOf("Card") != -1)
            //{
            //    rotation = new Vector3(0, 0, 90f);
            //}

            //Debug.Log(_transform.gameObject.name + ": " + _transform.localRotation + ". Local rotation start: " + rotation + ". z: " + rotation.z); 
            //Debug.Log(_transform.gameObject.name + ": " + _transform.localRotation + ". Quaternion.Euler end: " + Quaternion.Euler(rotation) + ". z: " + rotation.z);
            _transform.SetLocalPositionAndRotation(position, Quaternion.Euler(rotation));
        }

        public void MoveInstantly(Vector2 position, Vector3 rotation, Vector3 scaleVector)
        {
            _transform.SetPositionAndRotation(position, Quaternion.Euler(rotation));
            _transform.localScale = scaleVector;
        }

        public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            MoveLocalSmoothly(positon, rotation, duration);

            _transform.DOScale(scaleVector, duration);
        }

        public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration)
        {
            _transform.DOLocalMove(positon, duration);

            if (_transform.localRotation.eulerAngles != rotation)
                _transform.DOLocalRotate(rotation, duration);
        }

        //public void MoveLocalSmoothlyOnInstantlyRotation(Vector2 positon, Vector3 rotation, float duration)
        //{
        //    _transform.DOLocalMove(positon, duration);
        //    _transform.localRotation = Quaternion.Euler(rotation);
        //}

        public void MoveSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _transform.DOMove(positon, duration);
            _transform.DORotate(rotation, duration);
            _transform.DOScale(scaleVector, duration);
        }

        public void MoveLinear(Vector3 position, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            MoveLinear(position, maxRotationVector, duration);

            _transform.DOScale(scaleVector, duration).SetEase(Ease.Linear);
        }

        public void MoveLinear(Vector3 position, Vector3 maxRotationVector, float duration)
        {
            _transform.DOMove(position, duration).SetEase(Ease.Linear);
            _transform.DORotate(maxRotationVector, duration).SetEase(Ease.Linear);
        }
    }
}
