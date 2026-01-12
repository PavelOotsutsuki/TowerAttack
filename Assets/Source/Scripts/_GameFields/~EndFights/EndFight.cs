using System;
using System.Collections;
using System.Collections.Generic;
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

        private bool _isComplete;

        public void Init(IReadonlyFightResult fightResult)
        {
            gameObject.SetActive(false);
            _isComplete = false;

            _fightResult = fightResult;

            _panel.Init();
            _endFightLabel.Init();
            _exitFightLabel.Init();
            _exitFightMenu.Init();
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

            StartCoroutine(StartingEndFight(endFightLabelActivateData));
            _isComplete = true;
        }

        private IEnumerator StartingEndFight(EndFightLabelActivateData endFightLabelActivateData)
        {
            _panel.Show();

            yield return new WaitUntil(() => _panel.IsComplete);
            yield return new WaitForSeconds(_config.DelayBeforeEndFightLabelShow);

            _endFightLabel.Show(endFightLabelActivateData);

            yield return new WaitUntil(() => _endFightLabel.IsComplete);
            yield return new WaitForSeconds(_config.DelayBeforeExitFightLabelShow);

            _exitFightLabel.Show();

            yield return new WaitUntil(() => _exitFightLabel.IsComplete);

            _exitFightMenu.Activate();
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