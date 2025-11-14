using System;
using System.Collections.Generic;
using Cards.Animations;
using Cards.Animations.Curses;
using Cards.Effects;
using Cards.Insides;
using Cards.Views;
using Cards.Views.BigCardViews.Capabilities;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;
using UnityEngine.UI;

namespace Cards
{
    public class Card : MonoBehaviour, ICardTransformable, IDiscoverable, ICardNumber, IFeatureRechanger, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private CardPaper _cardPaper;
        [SerializeField] private CardConfig _config;
        [SerializeField] private Image _background;

        private readonly Vector3 _defaultScaleVector = new Vector3(1f,1f,1f);

        private CardSoundVolume _cardSoundVolume;
        private CardCharacter _character;
        private CardEffectManager _cardEffectManager;
        private CardViewData _viewData;
        private CardSpriteModeManager _cardSpriteModeManager;

        private ICardState _currentState;

        public ReadOnlyRectTransform RORTransform { get; private set; }
        public Movement CardMovement { get; private set; }
        public Vector3 DefaultScaleVector => _defaultScaleVector;
        public CardViewData ViewData => _viewData;
        public Image Background => _background;
        public SideType CurrentSide => _cardPaper.CurrentSide;
        public CardCapability CardCapability => _config.CardCapability;
        public bool IsCurse => _cardSpriteModeManager.IsCurse;
        public bool IsLuckyHorseshoe => _config.Effect.Type == EffectType.LuckyHorseshoe;

        internal void Init(IEffectFactory effectFactory, CardViewService cardViewService,
            ICardDragAndDropHandler cardDragAndDropHandler, CurseAnimator curseAnimator, CardSoundVolume cardSoundVolume,
            CardCapabilityDescription cardCapabilityDescription)
        {
            RORTransform = new ReadOnlyRectTransform(_rectTransform);
            _cardEffectManager = new CardEffectManager(_config.Effect, effectFactory);
            _viewData = new CardViewData(_config.CardViewConfig, _config.CardCapability);
            _cardSoundVolume = cardSoundVolume;

            _cardSpriteModeManager = new CardSpriteModeManager(_config.Effect.Type);
            _rectTransform.localScale = _defaultScaleVector;
            CardMovement = new Movement(_rectTransform);

            _cardPaper.Init(this, cardViewService, ViewData, _rectTransform, cardDragAndDropHandler, _cardSpriteModeManager,
                curseAnimator, cardCapabilityDescription);

            CreateCardCharacter();
            SetState(_cardPaper);
        }

        public void SetCurseMode()
        {
            if (IsCurse)
                return;

            _cardSpriteModeManager.SetCurseMode();
            _viewData.ChangeFeature(_viewData.Feature + "\n<CAP_4>ПРОКЛЯТ</CAP_4>");
            _config.SetCurseMode();
            _cardPaper.SetView(_viewData);
        }

        public bool IsSuccessChoice(int number)
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

            _cardEffectManager.Play(this);
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

        public void Fire(WaitForSeconds delay, CallbackHandler callbackHandler)
        {
            if (_currentState is not CardPaper)
            {
                throw new Exception("Try fire not CardPaper. Card state: " + _currentState.ToString());
            }

            _cardPaper.Fire(delay, callbackHandler);
        }

        public void Rise()
        {
            if (_currentState is not CardPaper)
                throw new Exception("Try rise not CardPaper. Card state: " + _currentState.ToString());

            if (_cardPaper.IsFired == false)
                throw new Exception("Try rise no fired Card");

            gameObject.SetActive(true);
            _rectTransform.localScale = _defaultScaleVector;
            _cardPaper.RiseFromTheAshes();
        }

        private void CreateCardCharacter()
        {
            _character = Instantiate(_config.CardCharacter, _rectTransform);
            _character.Init(_config.AwakeSound, _cardSoundVolume);
        }

        private void CheckStateByNull()
        {
            if (_currentState is null)
                throw new NullReferenceException("Current card state is null");
        }

        private void SetState(ICardState state)
        {
            _currentState?.Hide();
            _currentState = state;
            _currentState.Show();
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