using System.Collections;
using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using Zenject;
using GameFields.Persons.Common;
using GameFields.Persons.Discovers;
using Tools.Utils.FillComponents;
using System.Collections.Generic;
using GameFields.Decks;

namespace GameFields.StartFights
{
    public class StartFight : MonoBehaviour, IFightStep, IAutomaticFillComponents
    {
        [SerializeField] private StartFightPanel _startTowerCardSelectionPanel;
        [SerializeField] private StartFightLabel _startTowerCardSelectionLabel;
        [SerializeField] private WaitEnemySolutionLabel _waitEnemySolutionLabel;
        [SerializeField] private Seat[] _seats;
        [SerializeField] private Discover _discover;

        [SerializeField] private StartFightData _data;

        [SerializeField] private StartTowerCardSelectionPlayerData _playerData;
        [SerializeField] private StartTowerCardSelectionImitationData _imitationData;

        private StartTowerCardSelection _startTowerCardSelectionPlayer;
        private StartTowerCardSelection _startTowerCardSelectionImitation;

        private Deck _deck;

        public bool IsComplete => _startTowerCardSelectionPlayer.IsComplete && _startTowerCardSelectionImitation.IsComplete;

        [Inject]
        private void Construct(Deck deck)
        {
            _deck = deck;
        }

        public void Init(Player player, EnemyAI enemyAI)
        {
            _startTowerCardSelectionPanel.Init();
            _startTowerCardSelectionLabel.Init();
            _waitEnemySolutionLabel.Init();
            _discover.Init();

            _startTowerCardSelectionImitation = new StartTowerCardSelectionImitation(enemyAI, _data.FirstTurnCardsCount, _imitationData);
            _startTowerCardSelectionPlayer = new StartTowerCardSelectionPlayer(_deck, player, _seats, _discover, _playerData);
        }

        public void StartStep()
        {
            gameObject.SetActive(true);

            _startTowerCardSelectionPanel.Show();
            _startTowerCardSelectionLabel.Activate();

            WaitingViewStartLabel().ToUniTask();
        }

        private IEnumerator WaitingViewStartLabel()
        {
            yield return new WaitForSeconds(_data.WaitToStartDuration);

            _startTowerCardSelectionPlayer.StartProcess();
            _startTowerCardSelectionImitation.StartProcess();

            yield return new WaitUntil(() => _startTowerCardSelectionPlayer.IsComplete);

            if (_startTowerCardSelectionImitation.IsComplete == false)
            {
                yield return new WaitForSeconds(_data.DelayToViewEnemySolutionLabel);

                if (_startTowerCardSelectionImitation.IsComplete == false)
                {
                    _waitEnemySolutionLabel.Show();
                }
            }

            yield return new WaitUntil(() => _startTowerCardSelectionImitation.IsComplete);

            _waitEnemySolutionLabel.Hide();

            Deactivate();
        }

        private void Deactivate()
        {
            _startTowerCardSelectionPanel.Hide();

            WaitingToDestroy().ToUniTask();
        }

        //private void WaitToDestroy()
        //{
        //    WaitingToDestroy().ToUniTask();
        //}

        private IEnumerator WaitingToDestroy()
        {
            yield return new WaitUntil(() => _startTowerCardSelectionPanel.IsComplete);

            yield return new WaitUntil(() => _waitEnemySolutionLabel.IsComplete);

            Destroy(gameObject);
        }

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(StartFight))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineStartFightPanel(),
                DefineStartFightLabel(),
                DefineWaitEnemySolutionLabel(),
                DefineSeats(),
                DefineDiscover()
            };

            return list;
        }

        [ContextMenu(nameof(DefineStartFightPanel))]
        private ComponentAttachInfo DefineStartFightPanel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelectionPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineStartFightLabel))]
        private ComponentAttachInfo DefineStartFightLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _startTowerCardSelectionLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineWaitEnemySolutionLabel))]
        private ComponentAttachInfo DefineWaitEnemySolutionLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _waitEnemySolutionLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSeats))]
        private ComponentAttachInfo DefineSeats()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _seats);
        }

        [ContextMenu(nameof(DefineDiscover))]
        private ComponentAttachInfo DefineDiscover()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _discover, ComponentLocationTypes.InChildren);
        }

        #endregion 
    }
}