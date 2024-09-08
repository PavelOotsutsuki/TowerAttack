using System;
using System.Collections;
using Cards;
using Cysharp.Threading.Tasks;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    internal class DiscoverCardImitation: MonoBehaviour
    {
        [SerializeField] private float _growDuration = 1f;
        [SerializeField] private RectTransform _rectTransform;

        [SerializeField] private Image _frameImage;
        [SerializeField] private Color _selectedFrameColor;
        [SerializeField] private float _selectedWaitDuration = 1f;

        private Movement _movement;
        private Action _clickCallback;
        private float _scaleFactor;
        private Color _defaultColor;

        public void Init(Action clickCallback, float scaleFactor)
        {
            _defaultColor = _frameImage.color;
            _scaleFactor = scaleFactor;
            _rectTransform.rotation = Quaternion.identity;
            _rectTransform.localPosition = Vector3.zero;
            _clickCallback = clickCallback;
            _movement = new Movement(_rectTransform);

            Hide();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void Activate(float cardHeight, float cardWidth)
        {
            float bigHeight = cardHeight * _scaleFactor;
            float bigWidth = cardWidth * _scaleFactor;

            _frameImage.color = _defaultColor;
            _rectTransform.sizeDelta = new Vector2(bigWidth, bigHeight);
            _rectTransform.localPosition = Vector3.zero;

            Vector3 defaultScale = _rectTransform.localScale;
            _movement.MoveLocalInstantly(Vector3.zero, Quaternion.identity.eulerAngles, Vector3.zero);
            _movement.MoveLocalSmoothly(Vector3.zero, Quaternion.identity.eulerAngles, _growDuration, defaultScale);

            gameObject.SetActive(true);
        }

        public void StartClickImitation()
        {
            ClickingImitation().ToUniTask();
        }

        private IEnumerator ClickingImitation()
        {
            _frameImage.color = _selectedFrameColor;

            yield return new WaitForSeconds(_selectedWaitDuration);

            _clickCallback?.Invoke();
        }
    }
}