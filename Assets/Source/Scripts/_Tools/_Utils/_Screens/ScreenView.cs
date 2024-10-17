using UnityEngine;

namespace Tools.Utils.Screens
{
    public static class ScreenView
    {
        private static Vector2 _canvasReferenceResolution;

        public static void SetReferenceResolution(Vector2 referenceResolution)
        {
            _canvasReferenceResolution = referenceResolution;
        }

        public static float GetFactorX()
        {
            return Screen.width / _canvasReferenceResolution.x;
        }

        public static float GetFactorY()
        {
            return Screen.height / _canvasReferenceResolution.y;
        }
    }
}
