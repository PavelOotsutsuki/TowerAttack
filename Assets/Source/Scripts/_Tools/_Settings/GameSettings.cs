using UnityEngine;

namespace Tools.Settings
{
    public static class GameSettings
    {
        public const LanguageType Language = LanguageType.RU;
        public static readonly Vector2 CardSize = new Vector2(150f, 210f);
        public static readonly Vector2 CanvasReferenceResolution = new Vector2(1920f, 1080f);
    }
}