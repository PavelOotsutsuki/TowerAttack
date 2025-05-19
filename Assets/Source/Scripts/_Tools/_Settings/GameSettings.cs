using UnityEngine;

namespace Tools.Settings
{
    public static class GameSettings
    {
        public static Vector2 CardSize { get; private set; } = new Vector2(150f, 210f);
        public static Vector2 CanvasReferenceResolution { get; private set; } = new Vector2(1920f, 1080f);
    }
}