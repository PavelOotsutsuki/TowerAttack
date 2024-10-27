using UnityEngine;

namespace Tools
{
    public class ReadOnlyRectTransform : ReadOnlyTransform
    {
        private RectTransform _rectTransform;

        public ReadOnlyRectTransform(RectTransform rectTransform): base(rectTransform)
        {
            _rectTransform = rectTransform;
        }

        public float GetHeight()
        {
            return _rectTransform.rect.height;
        }

        public Vector2 GetSizeDelta()
        {
            return _rectTransform.sizeDelta;
        }

        public void SetSize(Vector2 size)
        {
            _rectTransform.sizeDelta = size;
        }
    }
}