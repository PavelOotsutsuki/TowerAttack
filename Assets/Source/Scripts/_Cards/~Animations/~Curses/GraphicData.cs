using UnityEngine;

namespace Cards.Animations.Curses
{
    internal class GraphicData
    {
        private readonly Color _color;

        private bool _isActive;

        public bool IsActive => _isActive;
        public Color Color => _color;

        public GraphicData(Color color)
        {
            _color = color;

            _isActive = true;
        }

        public void Activate()
        {
            _isActive = true;
        }

        public void Deactivate()
        {
            _isActive = false;
        }
    }
}