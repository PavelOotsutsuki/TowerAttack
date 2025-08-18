using System;
using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class Card : MonoBehaviour, ICardTransformable, ICardNumber, IFeatureRechanger, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardPaper _cardPaper;
        [SerializeField] private CardConfig _config;
        [SerializeField] private Image _background;

        private readonly Vector3 _defaultScaleVector = new Vector3(1f,1f,1f);

        private CardCharacter _character;
        private CardEffectManager _cardEffectManager;
        private CardViewData _viewData;

        private ICardState _currentState;

        public ReadOnlyRectTransform ReadOnlyRectTransform { get; private set; }
        public Movement CardMovement { get; private set; }
        public Vector3 DefaultScaleVector => _defaultScaleVector;
        public CardViewData ViewData => _viewData;
        public bool IsPlayingEffect => _cardEffectManager.IsPlayingEffect;
        public Image Background => _background;
        public SideType CurrentSide => _cardPaper.CurrentSide;
        //public EffectType EffectType => _config.Effect.Type;
        public EffectFeature EffectFeature => _config.EffectFeature;

        internal void Init(IEffectFactory effectFactory, CardViewService cardViewService,
            ICardDragAndDropHandler cardDragAndDropHandler)
        {
            ReadOnlyRectTransform = new ReadOnlyRectTransform(_rectTransform);
            _cardEffectManager = new CardEffectManager(_config.Effect, effectFactory);
            _viewData = new CardViewData(_config.CardViewConfig);

            _rectTransform.localScale = _defaultScaleVector;
            CardMovement = new Movement(_rectTransform);

            _cardPaper.Init(this, cardViewService, ViewData, _rectTransform, cardDragAndDropHandler);

            CreateCardCharacter();
            SetState(_cardPaper);
        }

        public bool IsSuccessAttack(int number)
        {
            return _config.CardViewConfig.Number == number;
        }

        public void EndDrag()
        {
            _cardPaper.EndDrag();
        }

        public void Kill()
        {
            Destroy(gameObject);
        }

        public void RechangeFeature(IEnumerable<TagValuePair> givenPairs = null)
        {
            _cardPaper.RechangeFeature(givenPairs);
        }

        //public void SetDragAndDropHandler(ICardDragAndDropHandler cardDragAndDropHandler)
        //{
        //    _cardPaper.SetDragAndDropHandler(cardDragAndDropHandler);
        //}

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

            _cardEffectManager.Play();
        }

        public bool TryDiscard()
        {
            if (_currentState is not CardCharacter)
            {
                throw new Exception("Try discard not CardCharacter. Card state: " + _currentState.ToString());
            }

            return _cardEffectManager.TryDiscard();
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
            if (_currentState is not CardPaper)
            {
                SetState(_cardPaper);
            }

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
            _currentState.Show();
        }

        public void Fire()
        {
            if (_currentState is not CardPaper)
            {
                throw new Exception("Try fire not CardPaper. Card state: " + _currentState.ToString());
            }

            _cardPaper.Fire();
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(Card))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineCardPaper()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCardPaper))]
        private ComponentAttachInfo DefineCardPaper()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _cardPaper, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}