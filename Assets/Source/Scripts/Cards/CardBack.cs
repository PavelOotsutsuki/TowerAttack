using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Tools;

namespace Cards
{
    public class CardBack : MonoBehaviour
    {
        private const float MaxRotation = 90f;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _invertDuration = 1f;

        private Vector3 _downWay;
        private Vector3 _maxRotationVector;
        private Vector3 _scaleVector;

        public void Init()
        {
            _maxRotationVector = new Vector3(MaxRotation, 0f, 0f);
            _downWay = new Vector3(_rectTransform.position.x, _rectTransform.position.y - _rectTransform.rect.height, _rectTransform.position.z);
            _scaleVector = _rectTransform.localScale * 2;
        }

        public void TakeCard()
        {
            _rectTransform.DOMove(_downWay, _invertDuration).SetEase(Ease.Linear);
            _rectTransform.DORotate(_maxRotationVector, _invertDuration).SetEase(Ease.Linear);
            _rectTransform.DOScale(_scaleVector, _invertDuration).SetEase(Ease.Linear);
        }

        //private IEnumerator TakeCardProcess()
        //{
            
        //}

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
    }
}

