using UnityEngine;

namespace Tools
{
    public class ReadOnlyRectTransform : ReadOnlyTransform
    {
        private readonly RectTransform _rectTransform;

        public ReadOnlyRectTransform(RectTransform rectTransform): base(rectTransform)
        {
            _rectTransform = rectTransform;
        }

        public float GetHeight()
        {
            return _rectTransform.rect.height;
        }

        public float GetWidth()
        {
            return _rectTransform.rect.width;
        }

        public Vector2 GetSizeDelta()
        {
            return _rectTransform.sizeDelta;
        }

        public void SetSize(Vector2 size)
        {
            _rectTransform.sizeDelta = size;
        }

        public Vector2 GetRightDownAnglePosition()
        {
            Vector2 position = GetPosition();
            float sizeX = GetWidth();
            float sizeY = GetHeight();

            return new Vector2(position.x + sizeX / 2, position.y - sizeY / 2);
        }
    }
}