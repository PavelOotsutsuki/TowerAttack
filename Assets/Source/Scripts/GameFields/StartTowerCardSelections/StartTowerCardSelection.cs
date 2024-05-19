using System.Collections;
using Tools;
using UnityEngine;
using GameFields.Seats;

namespace GameFields.StartTowerCardSelections
{
    public class StartTowerCardSelection : MonoBehaviour, IFightStep
    {
        [SerializeField] private StartTowerCardSelectionPanel _startTowerCardSelectionPanel;
        [SerializeField] private StartTowerCardSelectionLabel _startTowerCardSelectionLabel;
        [SerializeField] private Seat[] _seats;
        [SerializeField] private int _firstTurnCardsCount = 3;

        private IStartTowerCardSelectionListener _player;
        private IStartTowerCardSelectionListener _enemy;
        private bool _isCompletePlayer;
        private bool _isCompleteEnemy;
        private bool _isComplete; //Потом заменить 

        //public bool IsComplete => _player.IsTowerFilled; //&& _enemy.IsTowerFilled;
        public bool IsComplete => _isComplete; //&& _enemy.IsTowerFilled;

        public void Init(IStartTowerCardSelectionListener player, IStartTowerCardSelectionListener enemy)
        {
            _isComplete = false;

            _startTowerCardSelectionPanel.Init();
            _startTowerCardSelectionLabel.Init();

            _player = player;
            _enemy = enemy;

            InitSeats();
        }

        //public void PrepareToStart()
        //{
        //    _isComplete = false;
        //}

        public void StartStep()
        {
            gameObject.SetActive(true);

            _startTowerCardSelectionPanel.Activate();
            _startTowerCardSelectionLabel.Activate();

            StartCoroutine(StartProcess());
        }

        private IEnumerator StartProcess()
        {
            _player.DrawCards(_firstTurnCardsCount);
            _enemy.DrawCards(_firstTurnCardsCount);

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
