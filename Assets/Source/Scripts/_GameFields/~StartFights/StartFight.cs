using UnityEngine;
using Cysharp.Threading.Tasks;
using GameFields.Seats;
using Zenject;
using GameFields.Persons;
using GameFields.Persons.Discovers;
using Tools.Utils.FillComponents;
using System.Collections.Generic;
using GameFields.Decks;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using System.Threading;
using Tools;
using System;
using System.Reflection;

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
        private HandPlayer _handPlayer;
        private HandAI _handAI;
        private TowerPlayer _towerPlayer;
        private TowerAI _towerAI;
        //private SwitchRootPanel _switchRootPanel;
        private CancellationToken _gameFieldToken;

        public bool IsComplete => _startTowerCardSelectionPlayer.IsComplete && _startTowerCardSelectionImitation.IsComplete;

        [Inject]
        private void Construct(Deck deck, HandPlayer handPlayer, HandAI handAI, TowerPlayer towerPlayer, TowerAI towerAI)
        {
            _deck = deck;
            _handPlayer = handPlayer;
            _handAI = handAI;
            _towerPlayer = towerPlayer;
            _towerAI = towerAI;
            //_switchRootPanel = switchRootPanel;
        }

        public void Init(EnemyAI enemyAI, CancellationToken gameFieldToken)
        {
            _gameFieldToken = gameFieldToken;

            _startTowerCardSelectionPanel.Init();
            _startTowerCardSelectionLabel.Init();
            _waitEnemySolutionLabel.Init();
            _discover.Init(_gameFieldToken);

            _startTowerCardSelectionImitation = new StartTowerCardSelectionImitation(enemyAI, _handAI, _towerAI, _data.FirstTurnCardsCount, _imitationData);
            _startTowerCardSelectionPlayer = new StartTowerCardSelectionPlayer(_deck, _handPlayer, _towerPlayer, _seats, _discover, _playerData);
        }

        public void StartStep()
        {
            WaitingViewStartLabel(_gameFieldToken).Forget();
        }

        private async UniTask WaitingViewStartLabel(CancellationToken token)
        {
            await UniTask.WaitForSeconds(_data.WaitUntilBeginAllProcess, cancellationToken: token);

            //_switchRootPanel.Hide();

            //yield return new WaitUntil(() => _switchRootPanel.IsComplete);
            await UniTask.WaitForSeconds(_data.WaitAfterStartEndGamePanelComplete, cancellationToken: token);

            gameObject.SetActive(true);

            _startTowerCardSelectionPanel.Show(new CancellationTokenData(token));
            _startTowerCardSelectionLabel.Activate();

            await UniTask.WaitForSeconds(_data.WaitToStartDuration, cancellationToken: token);

            _startTowerCardSelectionPlayer.StartProcess(token);
            _startTowerCardSelectionImitation.StartProcess(token);

            await UniTask.WaitUntil(() => _startTowerCardSelectionPlayer.IsComplete, cancellationToken: token);

            if (_startTowerCardSelectionImitation.IsComplete == false)
            {
                await UniTask.WaitForSeconds(_data.DelayToViewEnemySolutionLabel, cancellationToken: token);

                if (_startTowerCardSelectionImitation.IsComplete == false)
                {
                    _waitEnemySolutionLabel.Show(new CancellationTokenData(token));
                }
            }

            await UniTask.WaitUntil(() => _startTowerCardSelectionImitation.IsComplete, cancellationToken: token);

            _waitEnemySolutionLabel.Hide(new CancellationTokenData(token));

            Deactivate(token);
        }

        private void Deactivate(CancellationToken token)
        {
            _startTowerCardSelectionPanel.Hide(new CancellationTokenData(token));

            WaitingToDestroy(token).Forget();
        }

        //private void WaitToDestroy()
        //{
        //    WaitingToDestroy().ToUniTask();
        //}

        private async UniTask WaitingToDestroy(CancellationToken token)
        {
            if (_gameFieldToken.IsCancellationRequested)
                return;

            try
            {
                await UniTask.WaitUntil(() => _startTowerCardSelectionPanel.IsComplete, cancellationToken: token);
                await UniTask.WaitUntil(() => _waitEnemySolutionLabel.IsComplete, cancellationToken: token);

                Destroy(gameObject);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
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