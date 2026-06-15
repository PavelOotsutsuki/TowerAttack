using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cards.Views;
using Cards.Views.BigCardViews.Capabilities;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.UI;
using Tools.Utils;
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
        //private Coroutine _hiddingCoroutine = null;
        private CancellationTokenSource _hiddingCTS;
        private CancellationTokenSource _showingCTS;
        private CancellationToken _fightToken;

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

        public void Init(CancellationToken fightToken)
        {
            _isComplete = true;

            _fightToken = fightToken;

            _rectTransform.rotation = Quaternion.identity;
            _canvasHeight = ScreenView.Y();
            _cardView.Init(_cardCapabilityDescription);

            gameObject.SetActive(false);
            _fadablePanel.Init();

            IsShown = false;
        }

        public void Show(TowerBigCardShowData data)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            if (IsShown == true)
                return;

            IsShown = true;

            Utils.DestroyCTS(ref _hiddingCTS);
            Utils.DestroyCTS(ref _showingCTS);
            _showingCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            _isComplete = false;

            _cardView.FillData(data.CardViewData);
            _sizeFactor = data.CardSize.x / data.CardSize.y;
            _bigHeight = _canvasHeight / _scaleFactor;
            _bigWidth = _bigHeight * _sizeFactor;
            _screenFactor = Screen.height / _canvasHeight;
            _rectTransform.position = new Vector2(data.PositionX, (_bigHeight / 2f + _canvasHeight / 10f) * _screenFactor);
            _rectTransform.sizeDelta = new Vector2(_bigWidth, _bigHeight);
            gameObject.SetActive(true);

            _fadablePanel.Show(new CancellationTokenData(_showingCTS.Token));
        }

        public void Hide()
        {
            if (_fightToken.IsCancellationRequested)
                return;

            if (IsShown == false)
                return;

            IsShown = false;

            Utils.DestroyCTS(ref _hiddingCTS);
            Utils.DestroyCTS(ref _showingCTS);
            _hiddingCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            Hidding(_hiddingCTS.Token).Forget();
        }

        private void OnDisable()
        {
            Utils.DestroyCTS(ref _hiddingCTS);
            Utils.DestroyCTS(ref _showingCTS);
        }

        private async UniTask Hidding(CancellationToken token)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            try
            {
                _fadablePanel.Hide(new CancellationTokenData(token));

                await UniTask.WaitUntil(() => _fadablePanel.IsComplete, cancellationToken: token);

                gameObject.SetActive(false);

                _isComplete = true;
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
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