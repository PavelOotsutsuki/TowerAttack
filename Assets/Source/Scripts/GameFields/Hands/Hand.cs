using System;
using System.Collections.Generic;
using UnityEngine;
using Cards;
using Tools;

namespace GameFields.Hands
{
    internal class Hand : MonoBehaviour
    {
        private const float StartRotation = 0f;

        [SerializeField] private float _startCardTranslateSpeed = 0.5f;
        [SerializeField] private float _startPositionX = 600f;
        [SerializeField] private float _startPositionY = 90f;
        [SerializeField, Min(0f)] private float _offsetX = 60f;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private HandSeatPool _handSeatPool;
        //[SerializeField, Min(0f)] private float _offsetLeftY = 2f;
        //[SerializeField, Min(0f)] private float _offsetRightY = 11f;
        //[SerializeField, Min(0f)] private float _offsetRotation = 10f;

        //[SerializeField, Min(0f)] private float _maxOffsetLeftY = 10f;
        //[SerializeField, Min(0f)] private float _maxOffsetRightY = 22f;
        //[SerializeField, Min(0f)] private float _maxOffsetRotation = 20f;
        [SerializeField] private float _handLength = 1175f;

        private List<Card> _cards;
        private List<HandSeat> _handSeats;

        public void Init(List<Card> startCards)
        {
            _cards = new List<Card>();
            _handSeats = new List<HandSeat>();
            _handSeatPool.Init();

            if (startCards == null)
            {
                return;
            }

            _cards = startCards;

            SortHandSeats();
        }

        public void Init(Card[] startCards)
        {
            _cards = new List<Card>();
            _handSeats = new List<HandSeat>();
            _handSeatPool.Init();

            if (startCards == null)
            {
                return;
            }

            foreach (Card card in startCards)
            {
                _cards.Add(card);
            }

            SortHandSeats();
        }

        public void Init()
        {
            _cards = new List<Card>();
            _handSeats = new List<HandSeat>();

            _handSeatPool.Init();
        }

        public void AddCard(Card card)
        {
            _cards.Add(card);

            if (_handSeatPool.TryGetHandSeat(out HandSeat handSeat))
            {
                Debug.Log("Add process");
                _handSeats.Add(handSeat);
                handSeat.SetCard(card, _startCardTranslateSpeed);
            }

            SortHandSeats();
        }

        public void RemoveCard(Card card)
        {
            _cards.Remove(card);

            if (TryFindHandSeat(out HandSeat handSeat, card))
            {
                _handSeats.Remove(handSeat);
                _handSeatPool.ReturnObjectInPool(handSeat.gameObject);
            }

            SortHandSeats();
        }


        //private void SortOddHand()
        //{
        //    for (int i = 0; i < _cards.Count; i++)
        //    {
        //        float offsetX = _offsetX;
        //        float positionX = _startPositionX + (_cards.Count / 2 - i) * offsetX * -1;
        //        float offsetRotation = (_cards.Count / 2 - i) * _offsetRotation;
        //        float offsetY;

        //        if (_cards.Count / 2 > i)
        //        {
        //            offsetY = _offsetLeftY;
        //        }
        //        else
        //        {
        //            offsetY = _offsetRightY * -1;
        //        }

        //        float positionY = _startPositionY + (_cards.Count / 2 - i) * offsetY * -1;
        //        Vector2 cardPosition = new Vector2(positionX + _rectTransform.rect.xMin, positionY + _rectTransform.rect.yMin);
        //        Vector3 rotation = new Vector3(0f, 0f, offsetRotation);

        //        _cards[i].TranslateLocalInto(cardPosition, rotation, _startCardTranslateSpeed);
        //    }
        //}

        //private void SortCards()
        //{
        //    if (_cards.Count <= 0)
        //    {
        //        return;
        //    }

        //    if (_cards.Count == 1)
        //    {
        //        _cards[0].TranslateLocalInto(new Vector2(_startPositionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin), new Vector3(0f, 0f, StartRotation), _startCardTranslateSpeed);
        //    }
        //    else if (_cards.Count % 2 == 1)
        //    {
        //        SortOddHand();
        //    }
        //    else
        //    {
        //        SortEvenHand();
        //    }
        //}

        private void SortHandSeats()
        {
            if (_handSeats.Count <= 0)
            {
                return;
            }

            float offsetX;
            float positionX;

            if (_handSeats.Count * _offsetX < _handLength / 2)
            {
                offsetX = _offsetX;
            }
            else
            {
                float xFactor = _offsetX * _handSeats.Count;
                float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

                offsetX = fullOffsetX / _handSeats.Count;
            }

            for (int i = 0; i < _handSeats.Count; i++)
            {
                positionX = _startPositionX + ((_handSeats.Count - 1) / 2f - i) * offsetX * -1;
                _handSeats[i].SetLocalPositionValues(new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin), new Vector3(0f, 0f, StartRotation));
                //_cards[i].TranslateLocalInto(new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin), new Vector3(0f, 0f, StartRotation), _startCardTranslateSpeed);
            }
        }

        private bool TryFindHandSeat(out HandSeat findedHandSeat, Card card)
        {
            foreach (HandSeat handSeat in _handSeats)
            {
                if (handSeat.Card == card)
                {
                    findedHandSeat = handSeat;
                    return true;
                }
            }

            findedHandSeat = null;
            return false;
        }

        //private void SortCards()
        //{
        //    if (_cards.Count <= 0)
        //    {
        //        return;
        //    }

        //    float offsetX;
        //    float positionX;

        //    if (_cards.Count * _offsetX < _handLength / 2)
        //    {
        //        offsetX = _offsetX;
        //    }
        //    else
        //    {
        //        float xFactor = _offsetX * _cards.Count;
        //        float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

        //        offsetX = fullOffsetX / _cards.Count;
        //    }

        //    for (int i = 0; i < _cards.Count; i++)
        //    {
        //        positionX = _startPositionX + ((_cards.Count - 1) / 2f - i) * offsetX * -1;
        //        _cards[i].TranslateLocalInto(new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin), new Vector3(0f, 0f, StartRotation), _startCardTranslateSpeed);
        //    }
        //}

        //private void SortFewCards()
        //{
        //    for (int i = 0; i < _cards.Count; i++)
        //    {
        //        float positionX = _startPositionX + (_cards.Count / 2 - i) * _offsetX * -1;
        //        _cards[i].TranslateLocalInto(new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin), new Vector3(0f, 0f, StartRotation), _startCardTranslateSpeed);
        //    }
        //}

        //private void SortOddHand()
        //{
        //    float offsetX = _offsetX;

        //    for (int i = 0; i < _cards.Count; i++)
        //    {
        //        float positionX = _startPositionX + (_cards.Count / 2 - i) * offsetX * -1;
        //        float offsetRotation = StartRotation + _maxOffsetRotation / (_cards.Count / 2) * (_cards.Count / 2 - i);
        //        float offsetY;

        //        if (_cards.Count / 2 > i)
        //        {
        //            offsetY = _maxOffsetLeftY;
        //        }
        //        else
        //        {
        //            offsetY = _maxOffsetRightY * -1;
        //        }

        //        float positionY = _startPositionY - offsetY / (_cards.Count / 2) * (_cards.Count / 2 - i);
        //        Vector2 cardPosition = new Vector2(positionX + _rectTransform.rect.xMin, positionY + _rectTransform.rect.yMin);
        //        Vector3 rotation = new Vector3(0f, 0f, offsetRotation);

        //        _cards[i].TranslateLocalInto(cardPosition, rotation, _startCardTranslateSpeed);
        //    }
        //}

        //private void SortOddHand()
        //{
        //    float offsetX;
        //    float positionX;

        //    if (_cards.Count * _offsetX < _handLength / 2)
        //    {
        //        offsetX = _offsetX;
        //    }
        //    else
        //    {
        //        float xFactor = _offsetX * _cards.Count;
        //        float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

        //        offsetX = fullOffsetX / _cards.Count;
        //    }

        //    for (int i = 0; i < _cards.Count; i++)
        //    {
        //        positionX = _startPositionX + (_cards.Count / 2 - i) * offsetX * -1;
        //        _cards[i].TranslateLocalInto(new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin), new Vector3(0f, 0f, StartRotation), _startCardTranslateSpeed);
        //    }
        //}

        //private void SortEvenHand()
        //{
        //    float offsetX;
        //    float positionX;

        //    if (_cards.Count * _offsetX < _handLength / 2)
        //    {
        //        offsetX = _offsetX;
        //    }
        //    else
        //    {
        //        float xFactor = _offsetX * _cards.Count;
        //        float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

        //        offsetX = fullOffsetX / _cards.Count;
        //    }

        //    for (int i = 0; i < _cards.Count; i++)
        //    {
        //        positionX = _startPositionX + ((_cards.Count - 1) / 2f - i) * offsetX * -1;
        //        _cards[i].TranslateLocalInto(new Vector2(positionX + _rectTransform.rect.xMin, _startPositionY + _rectTransform.rect.yMin), new Vector3(0f, 0f, StartRotation), _startCardTranslateSpeed);
        //    }
        //}

        //private float DefineOddOffsetX(float count)
        //{
        //    if (_cards.Count * _offsetX / 2 < _handLength)
        //    {
        //        return _offsetX;
        //    }

        //    float x = _offsetX * count;
        //    float fullOffsetX = _handLength * x / (x + _handLength / 2);


        //}

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
            DefineHandSeatPool();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineHandSeatPool))]
        private void DefineHandSeatPool()
        {
            AutomaticFillComponents.DefineComponent(this, ref _handSeatPool, ComponentLocationTypes.InThis);
        }
    }
}

