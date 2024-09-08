using System;
using Tools;
using UnityEngine;

namespace Cards
{
    public class Card : MonoBehaviour, ICardTransformable
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardPaper _cardPaper;
        [SerializeField] private CardConfig _config;
        //[SerializeField] private DiscoverCard _discoverCard;
        [SerializeField] private Vector3 _defaultScaleVector;

        private CardCharacter _character;
        private int _effectCounter;
        private IEffectFactory _effectFactory;
        private Effect _effect;

        private ICardState _currentState;

        public RectTransform Transform => _rectTransform;
        public CardMovement CardMovement { get; private set; }
        public Vector3 DefaultScaleVector => _defaultScaleVector;
        public CardViewConfig ViewConfig => _config.CardViewConfig;
        public bool IsPlayingEffect
        {
            get
            {
                if (_effect is not null)
                {
                    return _effect.IsPlayed;
                }

                return false;
            }
        }

        internal void Init(IEffectFactory effectFactory, CardViewService cardViewService, Transform dragContainer)
        {
            _effectFactory = effectFactory;

            _rectTransform.localScale = _defaultScaleVector;
            CardMovement = new CardMovement(_rectTransform);

            _cardPaper.Init(this, cardViewService, ViewConfig, dragContainer, _rectTransform);
            //_discoverCard.Init(ViewConfig, _rectTransform, _cardPaper);

            CreateCardCharacter();
            SetState(_cardPaper);
        }

        public void EndDrag()
        {
            _cardPaper.EndDrag();
        }

        public void SetDragAndDropListener(ICardDragAndDropListener cardDragAndDropListener)
        {
            _cardPaper.SetDragAndDropListener(cardDragAndDropListener);
        }

        //public void Discover(Transform parent)
        //{
        //    _discoverCard.transform.SetParent(parent);

        //    SetState(_discoverCard);
        //}

        public Vector3 GetPosition()
        {
            return _rectTransform.position;
        }

        public void Play()
        {
            CheckStateByNull();

            if (_currentState is not CardPaper)
            {
                throw new Exception("Try play not CardPaper. Card state: " + _currentState.ToString());
            }

            if (_character == null)
            {
                CreateCardCharacter();
            }

            SetState(_character);
            _effect = _effectFactory.Create(_config.Effect.Type);
            _effectCounter = _config.Effect.Duration;
        }

        public bool TryDiscard()
        {
            if (_currentState is not CardCharacter)
            {
                throw new Exception("Try discard not CardCharacter. Card state: " + _currentState.ToString());
            }

            _effectCounter--;

            if (_effectCounter <= 0)
            {
                _effect?.End();
                _effect = null;
                return true;
            }

            return false;
        }

        public void SetDiscardSide()
        {
            CheckStateByNull();

            if (_currentState is not CardCharacter)
            {
                throw new Exception("Try discard not CardCharacter. Card state: " + _currentState.ToString());
            }

            SetState(_cardPaper);
        }

        public void SetSide(SideType sideType)
        {
            _cardPaper.SetSide(sideType);
        }

        public void SetActiveInteraction(bool isActive)
        {
            _cardPaper.SetActiveInteraction(isActive);
        }

        private void CreateCardCharacter()
        {
            _character = Instantiate(_config.CardCharacter, _rectTransform);
            _character.Init(_config.AwakeSound);
        }

        private void CheckStateByNull()
        {
            if (_currentState is null)
            {
                throw new NullReferenceException("Current card state is null");
            }
        }

        private void SetState(ICardState state)
        {
            _currentState?.Hide();
            _currentState = state;
            _currentState.View();
        }
    }
}
