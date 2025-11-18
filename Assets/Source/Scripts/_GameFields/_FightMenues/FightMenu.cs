using System.Collections;
using System.Collections.Generic;
using Cards;
using Cysharp.Threading.Tasks;
using GameFields.InputSettings;
using GameFields.Persons.Commons;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.FightMenues
{
    public class FightMenu : MonoBehaviour, IWorkable, ICompletable, IFightMenuInputActivateWatcher, IAutomaticFillComponents
    {
        [SerializeField] private FightMenuLabel _fightMenuLabel;
        [SerializeField] private FightMenuPanel _fightMenuPanel;
        [SerializeField] private FightMenuButtonsPanelRoot _fightMenuButtonsPanelRoot;
        [SerializeField] private CanvasGroup _canvasGroup;

        private InputRoot _inputRoot;
        //private float _currentTimeScale;

        private bool _isComplete;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete => _isComplete;

        public IFocusedButtonEnterHandler CurrentFightMenuButtonInputHandler => _fightMenuButtonsPanelRoot.CurrentFightMenuButtonInputHandler;

        public void Init(InputRoot inputRoot, LoseActions playerLoseActions, IVolume cardVolume, IVolume musicVolume,
            CardCapabilityDescription cardCapabilityDescription)
        {
            gameObject.SetActive(false);
            _isComplete = true;
            IsActive = false;
            _canvasGroup.blocksRaycasts = true;

            _inputRoot = inputRoot;

            _fightMenuLabel.Init();
            _fightMenuPanel.Init();
            _fightMenuButtonsPanelRoot.Init(playerLoseActions, this, cardVolume, musicVolume, cardCapabilityDescription);
        }

        public void Activate()
        {
            if (IsActive == true || _isComplete == false)
                return;

            _inputRoot.Pause();
            _isComplete = false;
            IsActive = true;

            gameObject.SetActive(true);

            //_fightMenuLabel.Show();
            //_fightMenuPanel.Show();
            Activating().ToUniTask();
            //_selectResult = new SelectResult();

            //SelectNumberPanelActivateData numberPanelActivateData = new SelectNumberPanelActivateData(activateData.NeedSelect, activateData.RestrictionType, _selectResult);
            //_selectNumberPanel.Activate(numberPanelActivateData);
        }

        public void Deactivate()
        {
            if (IsActive == false || _isComplete == false)
                return;

            IsActive = false;
            _isComplete = false;
            _inputRoot.Pause();
            _inputRoot.DeactivateFightMenu();
            //Time.timeScale = _currentTimeScale;

            Deactivating().ToUniTask();
        }

        private IEnumerator Activating()
        {
            _fightMenuLabel.Show();
            _fightMenuPanel.Show();
            _fightMenuButtonsPanelRoot.Activate();

            yield return new WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && _fightMenuButtonsPanelRoot.IsComplete);

            _inputRoot.ActivateFightMenu();

            //_currentTimeScale = Time.timeScale;
            //Time.timeScale = 0;
            _isComplete = true;
        }

        private IEnumerator Deactivating()
        {
            _fightMenuLabel.Hide();
            _fightMenuPanel.Hide();
            _fightMenuButtonsPanelRoot.Deactivate();

            yield return new WaitUntil(() => _fightMenuLabel.IsComplete && _fightMenuPanel.IsComplete && _fightMenuButtonsPanelRoot.IsComplete);

            gameObject.SetActive(false);
            _inputRoot.DeactivateFightMenu();

            //SetSelectResultData setSelectResultData;

            //if (_selectResult.IsSelectSuccess)
            //{
            //    setSelectResultData = new SetSelectResultData(ResultType.Success);
            //    //_selectResultHandler.SuccessChoice();
            //}
            //else
            //{
            //    setSelectResultData = new SetSelectResultData(ResultType.Falled);
            //    //_selectResultHandler.FalledChoice();
            //}

            //SelectResultHandler.SetResult(_selectResult.Data);
            //_selectResultHandler.SetResult(_selectResult.Data);

            //yield return new WaitUntil(() => _selectResultHandler.IsComplete);

            _isComplete = true;
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(FightMenu))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFightMenuLabel(),
                DefineFightMenuPanel(),
                DefineFightMenuButtonsPanel(),
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFightMenuLabel))]
        private ComponentAttachInfo DefineFightMenuLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightMenuPanel))]
        private ComponentAttachInfo DefineFightMenuPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineFightMenuButtonsPanel))]
        private ComponentAttachInfo DefineFightMenuButtonsPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fightMenuButtonsPanelRoot, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}
