using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Menues;
using Tools;
using Tools.UI;
using Tools.Utils;
using UnityEngine;
using ISelectHandler = Menues.ISelectHandler;

namespace StartMenues
{
    public class StartMenuMainButtonsPanel : CustomFocusMenuButtonsPanel//, IAutomaticFillComponents
    {
        //[SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private ConfirmableFocusableButton _playButton;
        [SerializeField] private ConfirmableFocusableButton _campaignButton;
        [SerializeField] private ConfirmableFocusableButton _collectionButton;
        [SerializeField] private ConfirmableFocusableButton _achievementsButton;
        [SerializeField] private ConfirmableFocusableButton _rulesButton;
        [SerializeField] private ConfirmableFocusableButton _settingsButton;
        [SerializeField] private ConfirmableFocusableButton _exitButton;

        private Action _onPlayClick;
        private IHidable _startMenuDeactivatable;
        private ICompletable _startMenuCompletable;

        //private bool _isComplete;
        //private ConfirmableFocusableButton _currentFocusedButton;
        //private ISelectHandler _selectHandler;

        //public override bool? IsActive { get; protected set; } = null;
        //public bool IsComplete => _isComplete;

        public void Init(Action onSettingsButtonClick, Action onRulesButtonClick, Action onPlayClick,
            StartMenu startMenuDeactivatable)
        {
            //_isComplete = true;
            //_fadablePanel.Init();
            _onPlayClick = onPlayClick;
            _startMenuDeactivatable = startMenuDeactivatable;
            _startMenuCompletable = startMenuDeactivatable;

            List<ConfirmableFocusableButton> focusableButtons = new List<ConfirmableFocusableButton>()
            {
                _playButton,
                _campaignButton,
                _collectionButton,
                _achievementsButton,
                _rulesButton,
                _settingsButton,
                _exitButton
            };

            ISelectHandler selectHandler = new MainButtonsPanelSelectHandler(focusableButtons);
            base.Init(selectHandler, focusableButtons);


            _playButton.Init(this, StartPlaying);
            _campaignButton.Init(this, null);
            _collectionButton.Init(this, null);
            _achievementsButton.Init(this, null);
            _rulesButton.Init(this, onRulesButtonClick);
            _settingsButton.Init(this, onSettingsButtonClick);
            _exitButton.Init(this, Utils.Quit);

            _campaignButton.SetDisableView();
            _collectionButton.SetDisableView();
            _achievementsButton.SetDisableView();
        }

        private void StartPlaying()
        {
            StartingPlaying().ToUniTask();
        }

        private IEnumerator StartingPlaying()
        {
            _startMenuDeactivatable.Hide();

            yield return new WaitUntil(() => _startMenuCompletable.IsComplete);
            //yield return new WaitForSeconds(1f);

            _onPlayClick?.Invoke();
        }
    }
}