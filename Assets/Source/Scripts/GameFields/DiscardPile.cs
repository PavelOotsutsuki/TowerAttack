using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace GameFields.DiscardPiles
{
    public class DiscardPile : MonoBehaviour
    {
        private const float MinCoordinateX = 0f;
        private const float MinCoordinateY = 0f;
        private const float CenterRotation = 90f;

        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _cardRotationOffset = 45f;

        private float _maxCoordinateX;
        private float _maxCoordinateY;

        public void Init()
        {
            _maxCoordinateX = _rectTransform.rect.width;
            _maxCoordinateY = _rectTransform.rect.height;
        }

        private Vector3 FindCardPosition()
        {
            float xCoordinate = Random.Range(MinCoordinateX, _maxCoordinateX);
            float yCoordinate = Random.Range(MinCoordinateY, _maxCoordinateY);

            return new Vector3(xCoordinate, yCoordinate, 0f);
        }

        private Vector3 FindCardRotation()
        {
            float zRotation = Random.Range(CenterRotation - _cardRotationOffset, CenterRotation + _cardRotationOffset);

            return new Vector3(0f, 0f, zRotation);
        }

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
