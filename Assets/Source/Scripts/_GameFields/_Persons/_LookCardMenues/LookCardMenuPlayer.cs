using System;
using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.InputSettings;
using GameFields.Persons.Discovers;
using GameFields.Seats;
using Tools;
using Tools.Settings;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuPlayer : MonoBehaviour, ILookCardMenu, IAutomaticFillComponents
    {
        //[SerializeField] private LookCardMenuSeat[] _seats;
        [SerializeField] private LookCardMenuSeatPanelRoot _seatPanelRoot;
        [SerializeField] private LookCardMenuPanel _lookCardMenuPanel;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private LookCardMenuButton _lookCardMenuButton;
        //[SerializeField] private float _offset = 400f;
        //[SerializeField] private float _positionY = 0f;

        //private SeatPool _seatPool;
        private InputRoot _inputRoot;
        //private Vector2 _defaultCardSize;
        private bool _isComplete;

        //public int MaxSeats => _seats.Length;
        public IPointerClickHandler LookCardMenuButton => _lookCardMenuButton;
        public bool IsComplete => _isComplete;

        public bool? IsActive { get; private set; } = null;

        public void Init(InputRoot inputRoot)
        {
            //_defaultCardSize = GameSettings.CardSize;
            _inputRoot = inputRoot;

            _canvasGroup.blocksRaycasts = true;
            _isComplete = false;

            _seatPanelRoot.Init();
            _lookCardMenuPanel.Init();
            _lookCardMenuButton.Init(this, inputRoot);

            gameObject.SetActive(false);
        }

        public void Activate(LookCardMenuActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _canvasGroup.blocksRaycasts = true;
            _isComplete = false;

            gameObject.SetActive(true);
            _inputRoot.SetInputType(InputType.LookCardMenu);

            _seatPanelRoot.Activate(data.LookCardMenuSeatPanelRootActivateData);

            //for (int i = 0; i < _cards.Count; i++)
            //{
            //    _seats[i].SetCard(_cards[i]);
            //}
            
            _lookCardMenuPanel.Show();

            StartCoroutine(ActivatingButton());
        }

        private IEnumerator ActivatingButton()
        {
            yield return new WaitForSeconds(2f);

            _lookCardMenuButton.Activate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _canvasGroup.blocksRaycasts = false;

            _seatPanelRoot.Deactivate();
            _lookCardMenuPanel.Hide();
            _lookCardMenuButton.Deactivate();

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => _lookCardMenuPanel.IsComplete && _lookCardMenuButton.IsComplete && _seatPanelRoot.IsComplete);

            //_handBlockable.Unblock();
            _isComplete = true;
            gameObject.SetActive(false);
        }

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