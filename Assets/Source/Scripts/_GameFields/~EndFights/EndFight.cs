using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.EndFights
{
    public class EndFight: MonoBehaviour, IFightStep, IAutomaticFillComponents
    {
        [SerializeField] private EndFightPanel _panel;
        [SerializeField] private EndFightLabel _endFightLabel;
        [SerializeField] private ExitFightLabel _exitFightLabel;
        [SerializeField] private ExitFightMenu _exitFightMenu;

        [SerializeField] private EndFightConfig _config;

        private IReadonlyFightResult _fightResult;

        private CancellationToken _gameFieldToken;
        private CancellationTokenSource _fightCTS;

        private bool _isComplete;

        public void Init(IReadonlyFightResult fightResult, Action onDestroyPrefab, CancellationToken gameFieldToken,
            CancellationTokenSource fightCTS)
        {
            gameObject.SetActive(false);
            _isComplete = false;

            _fightResult = fightResult;

            _gameFieldToken = gameFieldToken;
            _fightCTS = fightCTS;

            _panel.Init();
            _endFightLabel.Init(_gameFieldToken);
            _exitFightLabel.Init();
            _exitFightMenu.Init(onDestroyPrefab);
        }

        public bool IsComplete => _isComplete;

        public void StartStep()
        {
            gameObject.SetActive(true);

            EndFightLabelActivateData endFightLabelActivateData = _fightResult.Result switch
            {
                EndFightResults.PlayerWin => _config.PlayerWinData,
                EndFightResults.EnemyWin => _config.EnemyWinData,
                EndFightResults.Draw => _config.DrawData,
                _ => throw new ArgumentNullException("Invalid EndTurnResult")
            };

            Utils.DestroyCTS(ref _fightCTS);

            StartingEndFight(endFightLabelActivateData).Forget();
            _isComplete = true;
        }

        private async UniTask StartingEndFight(EndFightLabelActivateData endFightLabelActivateData)
        {
            try
            {
                _panel.Show(new CancellationTokenData(_gameFieldToken));

                await UniTask.WaitUntil(() => _panel.IsComplete, cancellationToken: _gameFieldToken);
                await UniTask.WaitForSeconds(_config.DelayBeforeEndFightLabelShow, cancellationToken: _gameFieldToken);

                _endFightLabel.Show(endFightLabelActivateData);

                await UniTask.WaitUntil(() => _endFightLabel.IsComplete, cancellationToken: _gameFieldToken);
                await UniTask.WaitForSeconds(_config.DelayBeforeExitFightLabelShow, cancellationToken: _gameFieldToken);

                _exitFightLabel.Show(new CancellationTokenData(_gameFieldToken));

                await UniTask.WaitUntil(() => _exitFightLabel.IsComplete, cancellationToken: _gameFieldToken);

                ExitFightMenuActivateData exitFightMenuActivateData = new ExitFightMenuActivateData(_fightResult.Result);
                _exitFightMenu.Activate(exitFightMenuActivateData);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(EndFight))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineEndFightPanel(),
                DefineEndFightLabel(),
                DefineExitFightLabel(),
                DefineExitFightMenu()
            };

            return list;
        }

        [ContextMenu(nameof(DefineEndFightPanel))]
        private ComponentAttachInfo DefineEndFightPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _panel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineEndFightLabel))]
        private ComponentAttachInfo DefineEndFightLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _endFightLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineExitFightLabel))]
        private ComponentAttachInfo DefineExitFightLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _exitFightLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineExitFightMenu))]
        private ComponentAttachInfo DefineExitFightMenu()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _exitFightMenu, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}