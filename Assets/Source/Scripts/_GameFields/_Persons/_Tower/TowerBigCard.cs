using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using Tools.Utils.Screens;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.Towers
{
    [RequireComponent(typeof(FadablePanel))]
    public class TowerBigCard : MonoBehaviour, IViewable<TowerBigCardShowData>, ICompletable, IAutomaticFillComponents
    {
        [SerializeField, Min(1f)] private float _scaleFactor = 2f;

        [SerializeField] private CardView _cardView;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private FadablePanel _fadablePanel;

        private CardCapabilityDescription _cardCapabilityDescription;

        private bool _isComplete;
        private Coroutine _hiddingCoroutine = null;

        private float _bigHeight;
        private float _bigWidth;
        private float _sizeFactor;
        private float _canvasHeight;
        private float _screenFactor;

        public bool? IsShown { get; private set; } = null;
        public bool IsComplete => _isComplete;

        [Inject]
        public void Construct(CardCapabilityDescription cardCapabilityDescription)
        {
            _cardCapabilityDescription = cardCapabilityDescription;
        }

        public void Init()
        {
            _isComplete = true;

            _rectTransform.rotation = Quaternion.identity;
            _canvasHeight = ScreenView.Y();
            _cardView.Init(_cardCapabilityDescription);

            gameObject.SetActive(false);
            _fadablePanel.Init();

            IsShown = false;
        }

        public void Show(TowerBigCardShowData data)
        {
            if (IsShown == true)
                return;

            IsShown = true;

            if (_hiddingCoroutine != null)
            {
                StopCoroutine(_hiddingCoroutine);
                _hiddingCoroutine = null;
            }

            _isComplete = false;

            _cardView.FillData(data.CardViewData);
            _sizeFactor = data.CardSize.x / data.CardSize.y;
            _bigHeight = _canvasHeight / _scaleFactor;
            _bigWidth = _bigHeight * _sizeFactor;
            _screenFactor = Screen.height / _canvasHeight;
            _rectTransform.position = new Vector2(data.PositionX, (_bigHeight / 2f + _canvasHeight / 10f) * _screenFactor);
            _rectTransform.sizeDelta = new Vector2(_bigWidth, _bigHeight);
            gameObject.SetActive(true);

            _fadablePanel.Show();
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            _hiddingCoroutine = StartCoroutine(Hidding());
        }

        private void OnDisable()
        {
            _hiddingCoroutine = null;
        }

        private IEnumerator Hidding()
        {
            _fadablePanel.Hide();

            yield return new WaitUntil(() => _fadablePanel.IsComplete);

            gameObject.SetActive(false);

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerBigCard))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineFadablePanel()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}