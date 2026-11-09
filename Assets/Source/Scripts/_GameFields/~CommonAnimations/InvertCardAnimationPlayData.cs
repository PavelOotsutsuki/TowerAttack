using UnityEngine;

namespace GameFields.CommonAnimations
{
    public class InvertCardAnimationPlayData
    {
        private Vector3 _startSidePosition;
        private Vector3 _startSideScale;
        private Vector3 _finishSidePosition;
        private Vector3 _finishSideScale;

        public InvertCardAnimationPlayData(Vector3 startSidePosition, Vector3 startSideScale,
            Vector3 finishSidePosition, Vector3 finishSideScale)
        {
            _startSidePosition = startSidePosition;
            _startSideScale = startSideScale;
            _finishSidePosition = finishSidePosition;
            _finishSideScale = finishSideScale;
        }

        public Vector3 StartSidePosition => _startSidePosition;
        public Vector3 StartSideScale => _startSideScale;
        public Vector3 FinishSidePosition => _finishSidePosition;
        public Vector3 FinishSideScale => _finishSideScale;
    }
}