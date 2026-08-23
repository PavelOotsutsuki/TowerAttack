using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.InputSettings;
using Servers;
using Tools;
using Tools.InputSettings;
using Tools.UI;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuPlayer : MonoBehaviour, ILookCardMenu, IInputLogicObject, IEnterPressHandler,
        IRightArrowPressHandler, ILeftArrowPressHandler, IAutomaticFillComponents
    {
        //[SerializeField] private LookCardMenuSeat[] _seats;
        [SerializeField] private LookCardMenuSeatPanelRoot _seatPanelRoot;
        [SerializeField] private LookCardMenuPanel _lookCardMenuPanel;
        [SerializeField] private LookCardMenuLabel _lookCardMenuLabel;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private LookCardMenuButton _lookCardMenuButton;
        [Inject] private FightProcessDBManager _fightProcessDBManager;
        //[SerializeField] private float _offset = 400f;
        //[SerializeField] private float _positionY = 0f;

        //private SeatPool _seatPool;
        private GameFieldInputRoot _inputRoot;
        private CancellationToken _fightToken;

        private CancellationTokenSource _currentCTS;
        //private Vector2 _defaultCardSize;
        private bool _isComplete;

        //public int MaxSeats => _seats.Length;
        //public IPointerClickHandler LookCardMenuButton => _lookCardMenuButton;
        //public IPointerClickHandler RightSwitch => _seatPanelRoot.RightSwitch;
        //public IPointerClickHandler LeftSwitch => _seatPanelRoot.LeftSwitch;

        public bool IsComplete => _isComplete;

        public bool? IsActive { get; private set; } = null;

        public void Init(GameFieldInputRoot inputRoot, CancellationToken fightToken)
        {
            //_defaultCardSize = GameSettings.CardSize;
            _inputRoot = inputRoot;
            _fightToken = fightToken;

            _canvasGroup.blocksRaycasts = true;
            _isComplete = false;

            _seatPanelRoot.Init(_fightToken);
            _lookCardMenuPanel.Init();
            _lookCardMenuLabel.Init();
            _lookCardMenuButton.Init(this, inputRoot, _fightToken);

            gameObject.SetActive(false);
        }

        public void Activate(LookCardMenuActivateData data)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            if (IsActive == true)
                return;

            IsActive = true;

            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, true, GetSerializedCards(data.Cards), "START", "LOOK");

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            _canvasGroup.blocksRaycasts = true;
            _isComplete = false;

            gameObject.SetActive(true);
            _inputRoot.SetInputType(this);

            _seatPanelRoot.Activate(data.LookCardMenuSeatPanelRootActivateData);

            //for (int i = 0; i < _cards.Count; i++)
            //{
            //    _seats[i].SetCard(_cards[i]);
            //}
            
            _lookCardMenuPanel.Show(new CancellationTokenData( _currentCTS.Token));
            _lookCardMenuLabel.Show(new LabelActivateDataAsync(data.LabelActivateData, _currentCTS.Token));

            ActivatingButton(_currentCTS.Token).Forget();
        }

        public void Deactivate()
        {
            if (_fightToken.IsCancellationRequested)
                return;

            if (IsActive == false)
                return;

            IsActive = false;

            _fightProcessDBManager.WriteFightProcessAction(Fight.TurnNumber, true, null, "END", "LOOK");

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            _canvasGroup.blocksRaycasts = false;

            _seatPanelRoot.Deactivate();
            _lookCardMenuPanel.Hide(new CancellationTokenData(_currentCTS.Token));
            _lookCardMenuLabel.Hide(new CancellationTokenData(_currentCTS.Token));
            _lookCardMenuButton.Deactivate();

            Deactivating(_currentCTS.Token).Forget();
        }

        void IEnterPressHandler.OnEnter()
        {
            _lookCardMenuButton.OnPointerClick(null);
        }

        void IRightArrowPressHandler.OnRightArrow()
        {
            _seatPanelRoot.RightSwitch.OnPointerClick(null);
        }

        void ILeftArrowPressHandler.OnLeftArrow()
        {
            _seatPanelRoot.LeftSwitch.OnPointerClick(null);
        }

        private async UniTask ActivatingButton(CancellationToken token)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            try
            {
                await UniTask.WaitForSeconds(2f, cancellationToken: token);

                _lookCardMenuButton.Activate();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private async UniTask Deactivating(CancellationToken token)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            try
            {
                await UniTask.WaitUntil(() => _lookCardMenuPanel.IsComplete && _lookCardMenuButton.IsComplete &&
                _seatPanelRoot.IsComplete && _lookCardMenuLabel.IsComplete, cancellationToken: token);

                //_handBlockable.Unblock();
                _isComplete = true;
                gameObject.SetActive(false);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private string GetSerializedCards(IEnumerable<Card> cards)
        {
            return JsonSerializer.Serialize(cards.Select(c => c.ViewData.Number));
        }

        //private void OnDisable()
        //{
        //    Utils.DestroyCTS(ref _currentCTS);
        //}

        //private void SortSeats()
        //{

        //}

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LookCardMenuPlayer))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLookCardMenuSeatPanelRoot(),
                DefineLookCardMenuPanel(),
                DefineLookCardMenuLabel(),
                DefineLookCardMenuButton(),
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineLookCardMenuSeatPanelRoot))]
        private ComponentAttachInfo DefineLookCardMenuSeatPanelRoot()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _seatPanelRoot, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineLookCardMenuPanel))]
        private ComponentAttachInfo DefineLookCardMenuPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lookCardMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineLookCardMenuLabel))]
        private ComponentAttachInfo DefineLookCardMenuLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lookCardMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineLookCardMenuButton))]
        private ComponentAttachInfo DefineLookCardMenuButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lookCardMenuButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}