using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cards;
using Tools;
using UnityEngine;
using Zenject;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuSeatPanelRoot : MonoBehaviour, ICompletable, IWorkable<LookCardMenuSeatPanelRootActivateData>
    {
        [SerializeField] private LookCardMenuSeatPanel _seatPanelTemplate;
        [SerializeField] private LookCardMenuSeatPanelContainer _seatPanelContainer;
        [SerializeField] private LookCardMenuSeatPanelSwitch _rightSwitch;
        [SerializeField] private LookCardMenuSeatPanelSwitch _leftSwitch;
        [SerializeField, Min(0)] private int _startCountPanels = 5;

        private readonly List<LookCardMenuSeatPanel> _seatPanels = new List<LookCardMenuSeatPanel>();

        private CardDescription _description;
        private int _currentPanelIndex;
        private int _currentMaxIndex;
        private bool _isComplete;

        public bool? IsActive { get; private set; } = null;

        public bool IsComplete => _isComplete;

        [Inject]
        public void Construct(CardDescription cardDescription)
        {
            _description = cardDescription;
        }

        public void Init()
        {
            for (int i = 0; i < _startCountPanels; i++)
            {
                CreatePanel();
            }

            _rightSwitch.Init(NextSwitch);
            _leftSwitch.Init(PreviousSwitch);
        }

        public void Activate(LookCardMenuSeatPanelRootActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

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

        private void CheckSwitches()
        {
            if (_currentPanelIndex == _currentMaxIndex)
            {
                SetSwitchState(_rightSwitch, false);
            }
            else
            {
                SetSwitchState(_rightSwitch, true);
            }

            if (_currentPanelIndex > 0)
            {
                SetSwitchState(_leftSwitch, true);
            }
            else
            {
                SetSwitchState(_leftSwitch, false);
            }
        }

        private void CreatePanel()
        {
            LookCardMenuSeatPanel lookCardMenuSeatPanel = Instantiate(_seatPanelTemplate, _seatPanelContainer.GetTransform());
            _seatPanels.Add(lookCardMenuSeatPanel);
            lookCardMenuSeatPanel.Init(_description);
        }
    }
}
