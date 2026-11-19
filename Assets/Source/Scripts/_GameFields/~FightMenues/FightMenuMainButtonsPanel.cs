using System;
using System.Collections.Generic;
using System.Threading;
using GameFields.Persons;
using Menues;
using Tools;
using Tools.UI;
using Tools.Utils;
using UnityEngine;

namespace GameFields.FightMenues
{
    public class FightMenuMainButtonsPanel : CustomFocusMenuButtonsPanel
    {
        [SerializeField] private ConfirmableFocusableButton _resumeButton;
        [SerializeField] private ConfirmableFocusableButton _rulesButton;
        [SerializeField] private ConfirmableFocusableButton _settingsButton;
        [SerializeField] private ConfirmableFocusableButton _capitulateButton;
        [SerializeField] private ConfirmableFocusableButton _exitButton;

        public void Init(LoseActions playerLoseActions, IDeactivatable fightMenuDeactivator, Action onSettingsButtonClick,
            Action onRulesButtonClick, CancellationToken fightToken)
        {
            List<ConfirmableFocusableButton> focusableButtons = new List<ConfirmableFocusableButton>()
            {
                _resumeButton,
                _rulesButton,
                _settingsButton,
                _capitulateButton,
                _exitButton
            };

            ISelectHandler selectHandler = new MainButtonsPanelSelectHandler(focusableButtons);
            base.Init(selectHandler, focusableButtons);

            _resumeButton.Init(this, () => fightMenuDeactivator.Deactivate());
            _rulesButton.Init(this, onRulesButtonClick);
            _settingsButton.Init(this, onSettingsButtonClick);
            _capitulateButton.Init(this, () =>
            {
                fightMenuDeactivator.Deactivate();
                playerLoseActions.Activate(new CancellationTokenData(fightToken));
            });
            _exitButton.Init(this, Utils.Quit);
        }
    }
}