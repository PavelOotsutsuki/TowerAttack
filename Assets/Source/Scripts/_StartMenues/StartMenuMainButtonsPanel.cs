using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Menues;
using Servers;
using Servers.DTO;
using Tools;
using Tools.Loads;
using Tools.UI;
using Tools.Utils;
using UnityEngine;
using Zenject;
using ISelectHandler = Menues.ISelectHandler;

namespace StartMenues
{
    public class StartMenuMainButtonsPanel : CustomFocusMenuButtonsPanel, IPreactivatable//, IAutomaticFillComponents
    {
        //[SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private ConfirmableFocusableButton _playButton;
        [SerializeField] private ConfirmableFocusableButton _campaignButton;
        [SerializeField] private ConfirmableFocusableButton _collectionButton;
        [SerializeField] private ConfirmableFocusableButton _achievementsButton;
        [SerializeField] private ConfirmableFocusableButton _rulesButton;
        [SerializeField] private ConfirmableFocusableButton _settingsButton;
        [SerializeField] private ConfirmableFocusableButton _exitButton;
        [SerializeField] private Label _labelLogin;
        [SerializeField] private Label _labelLvl;
        [SerializeField] private Label _labelEx;

        [Inject] private DBRoot _dBRoot;

        private Action<int> _onPlayClick;
        private IHidable _startMenuDeactivatable;
        private ICompletable _startMenuCompletable;
        private CancellationToken _startMenuToken;

        public bool IsPreactive { get; private set; } = false;

        //private bool _isComplete;
        //private ConfirmableFocusableButton _currentFocusedButton;
        //private ISelectHandler _selectHandler;

        //public override bool? IsActive { get; protected set; } = null;
        //public bool IsComplete => _isComplete;

        public void Init(Action onSettingsButtonClick, Action onRulesButtonClick, Action<int> onPlayClick,
            StartMenu startMenuDeactivatable, CancellationToken startMenuToken)
        {
            //_isComplete = true;
            //_fadablePanel.Init();
            _onPlayClick = onPlayClick;
            _startMenuDeactivatable = startMenuDeactivatable;
            _startMenuCompletable = startMenuDeactivatable;
            _startMenuToken = startMenuToken;

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

            _labelLogin.Init();
            _labelLvl.Init();
            _labelEx.Init();

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

        public override void Activate()
        {
            if (IsActive == true)
                return;

            if (IsPreactive == false)
                Debug.LogWarning("Предупреждение последовательности активации. Возможно, сначала необходимо преактивировать объект");

            IsPreactive = false;

            base.Activate();

            //CancellationToken token = this.destroyCancellationToken;

            //Activating(token).Forget();
        }

        public async UniTask Preactivate(CancellationToken token)
        {
            _labelLogin.SetText("Загрузка");
            _labelLvl.SetText("Загрузка");
            _labelEx.SetText("Загрузка");

            GetMainMenuUserDataDTO getMainMenuUserDataDTO = await _dBRoot.GetMainMenuUserData(token);

            _labelLogin.SetText(getMainMenuUserDataDTO.username);
            _labelLvl.SetText($"Lvl: {getMainMenuUserDataDTO.level}");
            _labelEx.SetText($"EX: {getMainMenuUserDataDTO.score}/{getMainMenuUserDataDTO.max_experience}");

            IsPreactive = true;
        }

        //private async UniTask Activating(CancellationToken token)
        //{

        //    await UniTask.Delay(2000);

        //    base.Activate();


        //    _labelLogin.SetText("Загрузка");
        //    _labelLvl.SetText("Загрузка");
        //    _labelEx.SetText("Загрузка");

        //    GetUserDTO getUserDTO = await _dBRoot.GetUserData(token);

        //    _labelLogin.SetText(getUserDTO.username);
        //    _labelLvl.SetText("Lvl: " + getUserDTO.level);
        //    _labelEx.SetText("EX: " + getUserDTO.score + "/100");
        //}

        private void StartPlaying()
        {
            StartingPlaying(_startMenuToken).Forget();
        }

        private async UniTask StartingPlaying(CancellationToken token)
        {
            _startMenuDeactivatable.Hide();

            await UniTask.WaitUntil(() => _startMenuCompletable.IsComplete, cancellationToken: token);
            //yield return new WaitForSeconds(1f);

            _onPlayClick?.Invoke(1);
        }
    }
}