using UnityEngine;

namespace Tools.Utils.Screens
{
    internal class ResolutionTypeData : IData
    {
        private readonly string _text;
        private readonly Vector2 _vector2;

        public ResolutionTypeData(string text, Vector2 vector2)
        {
            _text = text;
            _vector2 = vector2;
        }

        public string Text => _text;
        public Vector2 Vector2 => _vector2;
    }
}