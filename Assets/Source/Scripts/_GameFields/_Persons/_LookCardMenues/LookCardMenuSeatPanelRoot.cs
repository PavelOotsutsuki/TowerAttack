using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuSeatPanelRoot : MonoBehaviour, ICompletable, IWorkable<LookCardMenuSeatPanelRootActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private BigCard _bigCard;
        [SerializeField] private LookCardMenuSeatPanel _seatPanelTemplate;
        [SerializeField] private LookCardMenuSeatPanelContainer _seatPanelContainer;
        [SerializeField] private LookCardMenuSeatPanelRightSwitch _rightSwitch;
        [SerializeField] private LookCardMenuSeatPanelLeftSwitch _leftSwitch;
        [SerializeField, Min(0)] private int _startCountPanels = 5;
        [SerializeField] private StoneFrame _stoneFrame;

        private readonly List<LookCardMenuSeatPanel> _seatPanels = new List<LookCardMenuSeatPanel>();

        private CardDescription _description;
        private int _currentPanelIndex;
        private int _currentMaxIndex;
        private bool _isComplete;

        public IPointerClickHandler RightSwitch => _rightSwitch;
        public IPointerClickHandler LeftSwitch => _leftSwitch;

        public bool? IsActive { get; private set; } = null;

        public bool IsComplete => _isComplete;

        [Inject]
        public void Construct(CardDescription cardDescription)
        {
            _description = cardDescription;
        }

        public void Init()
        {
            //for (int i = 0; i < _startCountPanels; i++)
            //{
            //    CreatePanel();
            //}

            _rightSwitch.Init(NextSwitch);
            _leftSwitch.Init(PreviousSwitch);
            _stoneFrame.Init();
        }

        public void Activate(LookCardMenuSeatPanelRootActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _stoneFrame.Activate();

            SetSwitchState(_rightSwitch, false);
            SetSwitchState(_leftSwitch, false);

            //_currentPanelIndex = 0;
            _currentMaxIndex = -1;

            List<Card> cards = data.Cards.ToList();

            //if (cards.Count > _seatPanels[_currentPanelIndex].MaxSeats)
            //{
            //    _rightSwitch.gameObject.SetActive(true);
            //    //lookCardMenuSeatPanelActivateData
            //}

            DestroyPanels();

            _currentPanelIndex = -1;

            while (cards.Count > 0)
            {
                IEnumerable<Card> cardsForPanel;

                _currentPanelIndex++;

                if (_currentPanelIndex >= _seatPanels.Count)
                    CreatePanel();

                if (cards.Count > _seatPanels[_currentPanelIndex].MaxSeats)
                {
                    cardsForPanel = cards.GetRange(0, _seatPanels[_currentPanelIndex].MaxSeats);
                    cards.RemoveRange(0, _seatPanels[_currentPanelIndex].MaxSeats);
                }
                else
                {
                    cardsForPanel = cards.GetRange(0, cards.Count);
                    cards.Clear();
                }

                LookCardMenuSeatPanelActivateData lookCardMenuSeatPanelActivateData = new LookCardMenuSeatPanelActivateData(cardsForPanel);

                _seatPanels[_currentPanelIndex].Activate(lookCardMenuSeatPanelActivateData);
                _seatPanels[_currentPanelIndex].gameObject.SetActive(false);
            }

            _currentMaxIndex = _currentPanelIndex;
            _currentPanelIndex = 0;

            _seatPanels[_currentPanelIndex].gameObject.SetActive(true);

            CheckSwitches();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            _isComplete = false;

            _stoneFrame.Deactivate();

            foreach (LookCardMenuSeatPanel panel in _seatPanels)
            {
                panel.Deactivate();
            }

            SetSwitchState(_rightSwitch, false);
            SetSwitchState(_leftSwitch, false);

            StartCoroutine(Deactivating());
        }

        private void SetSwitchState(LookCardMenuSeatPanelSwitch mySwitch, bool isActive)
        {
            if (isActive)
            {
                mySwitch.gameObject.SetActive(true);
                mySwitch.Activate();
            }
            else
            {
                mySwitch.Deactivate();
                mySwitch.gameObject.SetActive(false);
            }
        }

        private IEnumerator Deactivating()
        {
            yield return new WaitUntil(() => _seatPanels.Any(p => p.IsComplete == false) == false);

            _isComplete = true;
        }

        private void NextSwitch()
        {
            _seatPanels[_currentPanelIndex].gameObject.SetActive(false);
            _currentPanelIndex++;

            if (_currentPanelIndex > _currentMaxIndex)
                _currentPanelIndex = _currentMaxIndex;

            _seatPanels[_currentPanelIndex].gameObject.SetActive(true);

            CheckSwitches();
        }

        private void PreviousSwitch()
        {
            _seatPanels[_currentPanelIndex].gameObject.SetActive(false);
            _currentPanelIndex--;

            if (_currentPanelIndex < 0)
                _currentPanelIndex = 0;

            _seatPanels[_currentPanelIndex].gameObject.SetActive(true);

            CheckSwitches();
        }

        private void HideCardHelpers()
        {
            _bigCard.Hide();
            _description.Hide();
        }

        private void CheckSwitches()
        {
            HideCardHelpers();

            SetSwitchState(_rightSwitch, _currentPanelIndex != _currentMaxIndex);
            SetSwitchState(_leftSwitch, _currentPanelIndex > 0);


            //if (_currentPanelIndex == _currentMaxIndex)
            //{
            //    SetSwitchState(_rightSwitch, false);
            //}
            //else
            //{
            //    SetSwitchState(_rightSwitch, true);
            //}

            //if (_currentPanelIndex > 0)
            //{
            //    SetSwitchState(_leftSwitch, true);
            //}
            //else
            //{
            //    SetSwitchState(_leftSwitch, false);
            //}
        }

        private void CreatePanel()
        {
            LookCardMenuSeatPanel lookCardMenuSeatPanel = Instantiate(_seatPanelTemplate, _seatPanelContainer.GetTransform());
            _seatPanels.Add(lookCardMenuSeatPanel);
            lookCardMenuSeatPanel.Init(_description, _bigCard);
        }

        private void DestroyPanels()
        {
            if (_seatPanels.Count > 0)
            {
                foreach (LookCardMenuSeatPanel panel in _seatPanels)
                {
                    Destroy(panel.gameObject);
                }

                _seatPanels.Clear();
            }
        }


        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LookCardMenuSeatPanelRoot))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLookCardMenuSeatPanelContainer(),
                DefineLookCardMenuSeatPanelRightSwitch(),
                DefineLookCardMenuSeatPanelLeftSwitch(),
                DefineStoneFrame()
            };

            return list;
        }

        [ContextMenu(nameof(DefineLookCardMenuSeatPanelContainer))]
        private ComponentAttachInfo DefineLookCardMenuSeatPanelContainer()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _seatPanelContainer, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineLookCardMenuSeatPanelRightSwitch))]
        private ComponentAttachInfo DefineLookCardMenuSeatPanelRightSwitch()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rightSwitch, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineLookCardMenuSeatPanelLeftSwitch))]
        private ComponentAttachInfo DefineLookCardMenuSeatPanelLeftSwitch()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _leftSwitch, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineStoneFrame))]
        private ComponentAttachInfo DefineStoneFrame()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _stoneFrame, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}