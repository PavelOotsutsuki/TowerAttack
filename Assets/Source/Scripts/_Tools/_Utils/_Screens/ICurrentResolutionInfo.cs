using UnityEngine;

namespace Tools.Utils.Screens
{
    public interface ICurrentResolutionInfo
    {
        public ResolutionType Type { get; }
        public string Text { get; }
        public Vector2 Vector { get; }
        public float FactorX { get; }
        public float FactorY { get; }
        public float X { get; }
        public float Y { get; }
    }
}