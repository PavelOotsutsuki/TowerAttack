using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Tools;
using Tools.Utils.Movements;
using UnityEngine;

namespace Cards
{
    public class CardMovement
    {
        //private static int _actionNumber;
        private Movement _movement;

        public CardMovement(Transform transform)
        {
            _movement = new Movement(transform);
            //_actionNumber = 0;
        }

        public void MoveLocalInstantly(Vector2 position, Vector3 rotation)
        {
            //_actionNumber++;
            //int thisNumber = _actionNumber;

            //while (_movement.IsActive == false)
            //{
                _movement.MoveLocalInstantly(position, rotation);
            //}
        }

        public void MoveInstantly(Vector2 position, Vector3 rotation, Vector3 scaleVector)
        {
            _movement.MoveInstantly(position, rotation, scaleVector);
        }

        public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _movement.MoveLocalSmoothly(positon, rotation, duration, scaleVector);
        }

        public void MoveLocalSmoothly(Vector2 positon, Vector3 rotation, float duration)
        {
            _movement.MoveLocalSmoothly(positon, rotation, duration);
        }

        public void MoveSmoothly(Vector2 positon, Vector3 rotation, float duration, Vector3 scaleVector)
        {
            _movement.MoveSmoothly(positon, rotation, duration, scaleVector);
        }

        public void MoveLinear(Vector3 position, Vector3 maxRotationVector, float duration, Vector3 scaleVector)
        {
            _movement.MoveLinear(position, maxRotationVector, duration, scaleVector);
        }

        public void MoveLinear(Vector3 position, Vector3 maxRotationVector, float duration)
        {
            _movement.MoveLinear(position, maxRotationVector, duration);
        }

        public void MoveLinear(Vector3 position, Vector3 maxRotationVector, float duration, Action onCompleteCallback)
        {
            _movement.MoveLinear(position, maxRotationVector, duration, onCompleteCallback);
        }

        //private IEnumerator MoveLocalInstantly(Vector2 position, Vector3 rotation, int startNumber)
        //{
        //    int thisNumber = startNumber;

        //    yield return new WaitUntil()
        //}
    }
}
