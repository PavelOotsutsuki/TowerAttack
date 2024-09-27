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
using GameFields.Persons.Discovers;

namespace GameFields.StartFights
{
    public class StartFight : MonoBehaviour, IFightStep
    {
        [SerializeField] private StartFightPanel _startTowerCardSelectionPanel;
        [SerializeField] private StartFightLabel _startTowerCardSelectionLabel;
        [SerializeField] private Seat[] _seats;
        [SerializeField] private Discover _discover;
        [SerializeField] private int _firstTurnCardsCount = 3;

        private List<StartTowerCardSelection> _startTowerCardSelections;

        private Deck _deck;

        public bool IsComplete
        {
            get
            {
                bool isComplete = true;

                foreach (StartTowerCardSelection startTowerCardSelection in _startTowerCardSelections)
                {
                    isComplete &= startTowerCardSelection.IsComplete;
                }

                return isComplete;
            }
        }

        [Inject]
        private void Construct(Deck deck)
        {
            _deck = deck;
        }

        public void Init(Player player, EnemyAI enemyAI)
        {
            _startTowerCardSelectionPanel.Init();
            _startTowerCardSelectionLabel.Init();
            _discover.Init();

            _startTowerCardSelections = new List<StartTowerCardSelection>
            {
                new StartTowerCardSelectionPlayer(_deck, player, _seats, _discover),
                new StartTowerCardSelectionImitation(enemyAI, _firstTurnCardsCount)
            };
        }

        public void StartStep()
        {
            gameObject.SetActive(true);

            _startTowerCardSelectionPanel.Activate();
            _startTowerCardSelectionLabel.Activate();

            WaitingViewStartLabel().ToUniTask();
        }

        private IEnumerator WaitingViewStartLabel()
        {
            yield return new WaitForSeconds(2f);

            foreach (StartTowerCardSelection startTowerCardSelection in _startTowerCardSelections)
            {
                startTowerCardSelection.StartProcess();
            }

            yield return new WaitUntil(() => IsComplete);

            Deactivate();
        }

        //private IEnumerator WaitingFillTowers()
        //{
        //    yield return new WaitUntil(() => IsComplete);

        //    Deactivate();
        //}

        private void Deactivate()
        {
            _startTowerCardSelectionPanel.Deactivate(() => Destroy(gameObject));

            //WaitingToDestroy().ToUniTask();
            //Destroy(gameObject);
        }

        //private IEnumerator WaitingToDestroy()
        //{
        //    yield return new WaitForSeconds(0.5f);

        //    Destroy(gameObject);
        //}

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFirstTurnLabel();
            DefineSeats();
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
