using System.Collections.Generic;
using Cards;
using GameFields.DiscardPiles;
using UnityEngine;

namespace GameFields.Persons.Fires
{
    public class FirePool : ICardSeatable
    {
        private const float CenterRotation = 90f;

        private readonly float _maxCoordinateX;
        private readonly float _maxCoordinateY;
        private readonly float _minCoordinateX;
        private readonly float _minCoordinateY;
        private readonly float _cardRotationOffset = 30f;

        private readonly List<Card> _fireList;
        private readonly Transform _parent;

        public FirePool(Transform parent)
        {
            _fireList = new List<Card>();
            _parent = parent;

            _maxCoordinateX = ((RectTransform)parent).rect.width / 2f;
            _maxCoordinateY = ((RectTransform)parent).rect.height / 2f;
            _minCoordinateX = _maxCoordinateX * -1;
            _minCoordinateY = _maxCoordinateY * -1;
        }

        public IReadOnlyList<Card> FireList => _fireList;
        public int Count => _fireList.Count;

        public void SeatCard(Card card)
        {
            card.gameObject.SetActive(false);

            _fireList.Add(card);

            Seat(card);
        }

        public void Remove(Card card)
        {
            card.Rise();

            _fireList.Remove(card);
        }

        public void Clear()
        {
            _fireList.Clear();
        }

        private void Seat(Card card)
        {
            card.ReadOnlyRectTransform.SetParent(_parent);
            card.CardMovement.MoveLocalInstantly(FindCardSeatPosition(), FindCardSeatRotation());
        }

        private Vector3 FindCardSeatPosition()
        {
            float xCoordinate = Random.Range(_minCoordinateX, _maxCoordinateX);
            float yCoordinate = Random.Range(_minCoordinateY, _maxCoordinateY);

            return new Vector3(xCoordinate, yCoordinate, 0f);
        }

        private Vector3 FindCardSeatRotation()
        {
            float zRotation = Random.Range(CenterRotation - _cardRotationOffset, CenterRotation + _cardRotationOffset);

            return new Vector3(0f, 0f, zRotation);
        }
    }
}