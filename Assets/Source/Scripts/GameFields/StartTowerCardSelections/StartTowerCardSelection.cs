using System.Collections;
using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using System.Collections.Generic;
using Cards;
using Zenject;

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
        private Deck _deck;

        private List<Card> _enemyCards;

        public bool IsComplete { get; private set; }

        [Inject]
        private void Construct(Deck deck)
        {
            _deck = deck;
        }

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

            StartPlayerProcess().ToUniTask();
            StartEnemyProcess();

            WaitingFillTowers().ToUniTask();
        }

        private IEnumerator StartPlayerProcess()
        {
            yield return new WaitForSeconds(1f);

            for (int i = 0; i < _firstTurnCardsCount; i++)
            {
                _seats[i].SetCard(_deck.TakeTopCard(), SideType.Front, 1.5f, 2f);
                yield return new WaitForSeconds(1f);
                //card.StartSelection();

            }
        }

        private IEnumerator WaitingFillTowers()
        {
            yield return new WaitUntil(() => IsComplete);

            Deactivate();
        }

        private void StartEnemyProcess()
        {
            _enemyCards = _personsState.Deactive.DrawCards(_firstTurnCardsCount, StartEnemyProcess);
        }

        private void StartingEnemyProcess()
        {
            StartingEnemyProcess(_enemyCards).ToUniTask();
        }

        private IEnumerator StartingEnemyProcess(List<Card> enemyCards)
        {
            yield return new WaitForSeconds(2f);

            for (int i = 0; i < _firstTurnCardsCount; i++)
            {
                yield return new WaitForSeconds(1f);
            }

            IsComplete = true;
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
