using UnityEngine;
using Tools.Settings;

namespace Tools.Utils.Screens
{
    public static class ScreenView
    {
        public static float GetFactorX()
        {
            return Screen.width / GameSettings.CanvasReferenceResolution.x;
        }

        public static float GetFactorY()
        {
            return Screen.height / GameSettings.CanvasReferenceResolution.y;
        }

        public static float X()
        {
            return GameSettings.CanvasReferenceResolution.x;
        }

        public static float Y()
        {
            return GameSettings.CanvasReferenceResolution.y;
        }
    }
}