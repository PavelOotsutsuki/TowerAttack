using System.Collections;
using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using Cards;
using GameFields.Persons;
using System;

namespace GameFields.StartTowerCardSelections
{
    public class StartTowerCardSelection : MonoBehaviour
    {
        [SerializeField] private StartTowerCardSelectionPanel _startTowerCardSelectionPanel;
        [SerializeField] private StartTowerCardSelectionLabel _startTowerCardSelectionLabel;
        [SerializeField] private Seat[] _seats;
        [SerializeField] private int _firstTurnCardsCount = 3;

        private IStartTowerCardSelectionListener _player;
        private IStartTowerCardSelectionListener _enemy;
        private bool _isCompletePlayer;
        private bool _isCompleteEnemy;

        //public bool IsComplete => _player.IsTowerFilled; //&& _enemy.IsTowerFilled;
        public bool IsComplete => true; //&& _enemy.IsTowerFilled;

        public void Init(IStartTowerCardSelectionListener player, IStartTowerCardSelectionListener enemy)
        {
            _startTowerCardSelectionPanel.Init();
            _startTowerCardSelectionLabel.Init();

            _player = player;
            _enemy = enemy;

            InitSeats();
        }

        public void Activate()
        {
            gameObject.SetActive(true);

            _startTowerCardSelectionPanel.Activate();
            _startTowerCardSelectionLabel.Activate();

            StartProcess().ToUniTask();
        }

        private IEnumerator StartProcess()
        {
            _player.StartTowerCardSelection(_firstTurnCardsCount);
            _enemy.StartTowerCardSelection(_firstTurnCardsCount);

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
