using System.Collections;
using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Seats;

namespace GameFields.StartTowerCardSelections
{
    public class StartTowerCardSelection : MonoBehaviour, IFightStep
    {
        [SerializeField] private StartTowerCardSelectionPanel _startTowerCardSelectionPanel;
        [SerializeField] private StartTowerCardSelectionLabel _startTowerCardSelectionLabel;
        [SerializeField] private Seat[] _seats;
        [SerializeField] private int _firstTurnCardsCount = 3;

        private IPersonsState _personsState;
        private bool _isCompletePlayer;
        private bool _isCompleteEnemy;

        public bool IsComplete { get; private set; }

        public void Init(IPersonsState personsState)
        {
            _personsState = personsState;
            
            IsComplete = false;

            _startTowerCardSelectionPanel.Init();
            _startTowerCardSelectionLabel.Init();

            InitSeats();
        }

        public void StartStep()
        {
            gameObject.SetActive(true);

            _startTowerCardSelectionPanel.Activate();
            _startTowerCardSelectionLabel.Activate();

            StartProcess().ToUniTask();
        }

        private IEnumerator StartProcess()
        {
            _personsState.Active.DrawCards(_firstTurnCardsCount);
            _personsState.Deactive.DrawCards(_firstTurnCardsCount);

            yield return new WaitUntil(() => IsComplete);

            Deactivate();
        }

        private void Deactivate()
        {
            _startTowerCardSelectionPanel.Deactivate(() => Destroy(gameObject));
        }

        private void InitSeats()
        {
            foreach (Seat seat in _seats)
            {
                seat.Init();
            }
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFirstTurnPanel();
            DefineFirstTurnLabel();
            DefineSeats();
        }

        [ContextMenu(nameof(DefineFirstTurnPanel))]
        private void DefineFirstTurnPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelectionPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFirstTurnLabel))]
        private void DefineFirstTurnLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelectionLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSeats))]
        private void DefineSeats()
        {
            AutomaticFillComponents.DefineComponent(this, ref _seats);
        }
    }
}
