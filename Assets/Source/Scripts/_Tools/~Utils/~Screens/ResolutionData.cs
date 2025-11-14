using UnityEngine;

namespace Tools.Utils.Screens
{
    internal class ResolutionData
    {
        private readonly Resolution _resolution;
        private readonly string _text;

        public ResolutionData(Resolution resolution)
        {
            _resolution = resolution;
            _text = resolution.width + "x" + resolution.height;
        }

        public Resolution Resolution => _resolution;
        public string Text => _text;
    }
}