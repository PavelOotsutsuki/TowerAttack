using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Servers;
using Tools;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;
using Zenject;

namespace GameFields.EndFights
{
    public class EndFight: MonoBehaviour, IFightStep, IAutomaticFillComponents
    {
        [SerializeField] private EndFightPanel _panel;
        [SerializeField] private EndFightLabel _endFightLabel;
        [SerializeField] private ExitFightLabel _exitFightLabel;
        [SerializeField] private ExitFightMenu _exitFightMenu;
        [SerializeField] private AddedExperienceLabel _addedExperienceLabel;

        [SerializeField] private EndFightConfig _config;

        [Inject] private DBRoot _dBRoot;

        private IReadonlyFightResult _fightResult;

        private CancellationToken _gameFieldToken;
        private CancellationTokenSource _fightCTS;

        private int? _addedXp = null;

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
            _addedExperienceLabel.Init(_gameFieldToken);
        }

        public bool IsComplete => _isComplete;

        public void StartStep()
        {
            gameObject.SetActive(true);

            Utils.DestroyCTS(ref _fightCTS);
            //_fightCTS?.Cancel();
            //_fightCTS?.Dispose();

            StartingEndFight(_fightResult.Result, _gameFieldToken).Forget();
            _isComplete = true;
        }

        private async UniTask StartingEndFight(EndFightResults result, CancellationToken token)
        {
            try
            {
                GetLastAddedExp(token).Forget();
                EndFightLabelActivateData endFightLabelActivateData = result switch
                {
                    EndFightResults.PlayerWin => _config.PlayerWinData,
                    EndFightResults.EnemyWin => _config.EnemyWinData,
                    EndFightResults.Draw => _config.DrawData,
                    _ => throw new ArgumentNullException("Invalid EndTurnResult: " + result)
                };

                _panel.Show(new CancellationTokenData(token));

                await UniTask.WaitUntil(() => _panel.IsComplete, cancellationToken: token);
                await UniTask.WaitForSeconds(_config.DelayBeforeEndFightLabelShow, cancellationToken: token);

                _endFightLabel.Show(endFightLabelActivateData);

                await UniTask.WaitUntil(() => _endFightLabel.IsComplete, cancellationToken: token);
                await UniTask.WaitForSeconds(_config.DelayBeforeAddedExperienceLabelShow, cancellationToken: token);
                await UniTask.WaitUntil(() => _addedXp.HasValue, cancellationToken: token);

                AddedExperienceLabelActivateData addedExperienceLabelActivateData = new AddedExperienceLabelActivateData(_addedXp.Value, result);
                _addedExperienceLabel.Show(addedExperienceLabelActivateData);

                await UniTask.WaitUntil(() => _addedExperienceLabel.IsComplete, cancellationToken: token);
                await UniTask.WaitForSeconds(_config.DelayBeforeExitFightLabelShow, cancellationToken: token);

                _exitFightLabel.Show(new CancellationTokenData(token));

                await UniTask.WaitUntil(() => _exitFightLabel.IsComplete, cancellationToken: token);

                ExitFightMenuActivateData exitFightMenuActivateData = new ExitFightMenuActivateData(result);
                _exitFightMenu.Activate(exitFightMenuActivateData);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private async UniTask GetLastAddedExp(CancellationToken token)
        {
            _addedXp = await _dBRoot.GetLastAddedExp(token);
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
                DefineExitFightMenu(),
                DefineAddedExperienceLabel()
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

        [ContextMenu(nameof(DefineAddedExperienceLabel))]
        private ComponentAttachInfo DefineAddedExperienceLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _addedExperienceLabel, ComponentLocationTypes.InChildren);
        }
        #endregion 
    }
}