using System.Collections;
using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using System.Collections.Generic;
using Cards;
using Zenject;
using GameFields.Persons.Towers;
using GameFields.Persons;

namespace GameFields.StartTowerCardSelections
{
    public class StartTowerCardSelection : MonoBehaviour, IFightStep
    {
        [SerializeField] private StartTowerCardSelectionPanel _startTowerCardSelectionPanel;
        [SerializeField] private StartTowerCardSelectionLabel _startTowerCardSelectionLabel;
        [SerializeField] private Seat[] _seats;
        [SerializeField] private int _firstTurnCardsCount = 3;

        private StartTowerCardSelectionPlayer _selectionPlayer;
        private StartTowerCardSelectionImitation _selectionImitation;

        //private List<>

        private Deck _deck;

        public bool IsComplete => _selectionImitation.IsComplete && _selectionPlayer.IsComplete;

        [Inject]
        private void Construct(Deck deck)
        {
            _deck = deck;
        }

        public void Init(Player player, EnemyAI enemyAI)
        {
            _startTowerCardSelectionPanel.Init();
            _startTowerCardSelectionLabel.Init();

            _selectionPlayer = new StartTowerCardSelectionPlayer(_deck, player, _seats);
            _selectionImitation = new StartTowerCardSelectionImitation(enemyAI, _firstTurnCardsCount);
        }

        public void StartStep()
        {
            gameObject.SetActive(true);

            _startTowerCardSelectionPanel.Activate();
            _startTowerCardSelectionLabel.Activate();

            _selectionPlayer.StartProcess();
            _selectionImitation.StartProcess();

            WaitingFillTowers().ToUniTask();
        }

        private IEnumerator WaitingFillTowers()
        {
            yield return new WaitUntil(() => IsComplete);

            Deactivate();
        }

        private void Deactivate()
        {
            _startTowerCardSelectionPanel.Deactivate(() => Destroy(gameObject));
        }

        #region AutomaticFillComponents

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

        #endregion 
    }
}
