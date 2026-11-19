using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;
using static TMPro.TMP_Dropdown;
using Tools.Utils.Screens;
using Tools.UI.Extendeds;
using System.Collections;

namespace Menues
{
    public class MenuSettingsButtonsPanel : MenuButtonsPanel, IAutomaticFillComponents
    {
        [SerializeField] private GoBackOnMainPanelButton _goBackOnMainPanelButton;
        [SerializeField] private Slider _cardVolumeSlider;
        [SerializeField] private Slider _musicVlumeSlider;
        [SerializeField] private ExtendedDropdown _screenDropdown;

        private IVolume _cardVolume;
        private IVolume _musicVolume;
        private ScreenRoot _screenRoot;

        //public override bool? IsActive { get; protected set; } = null;

        [Inject]
        private void Construct(ScreenRoot screenRoot)
        {
            _screenRoot = screenRoot;
        }

        public void Init(Action onClickGoBackOnMainPanelButton, IVolume cardVolume, IVolume musicVolume)
        {
            _goBackOnMainPanelButton.Init(onClickGoBackOnMainPanelButton);
            _cardVolume = cardVolume;
            _musicVolume = musicVolume;

            //SetCurrentValues();

            _cardVolumeSlider.onValueChanged.AddListener(OnCardSliderValueChanged);
            _musicVlumeSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);
            _screenDropdown.onValueChanged.AddListener(OnScreenDropdownValueChanged);
        }

        public void OnDestroy()
        {
            _cardVolumeSlider.onValueChanged.RemoveListener(OnCardSliderValueChanged);
            _musicVlumeSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
            _screenDropdown.onValueChanged.RemoveListener(OnScreenDropdownValueChanged);

            _screenDropdown.options.Clear();
        }

        public override void Activate()
        {
            if (IsActive == true)
                return;

            base.Activate();

            SetCurrentValues();

            ActivatingGoBackOnMainPanelButton().Forget();
            //StartCoroutine(ActivatingGoBackOnMainPanelButton());
        }

        private void OnCardSliderValueChanged(float value)
        {
            _cardVolume.SetVolumePercent(value);
        }

        private void OnMusicSliderValueChanged(float value)
        {
            _musicVolume.SetVolumePercent(value);
        }

        private void SetCurrentValues()
        {
            _cardVolumeSlider.value = _cardVolume.Percent;
            _musicVlumeSlider.value = _musicVolume.Percent;

            _screenDropdown.options.Clear();

            IEnumerable<Resolution> resolutions = _screenRoot.Resolutions;
            List<OptionData> optionDatas = new List<OptionData>();
            int currentIndex = -1;

            foreach (Resolution resolution in resolutions)
            {
                OptionData optionData = new OptionData(_screenRoot.GetResolutionData(resolution));
                optionDatas.Add(optionData);

                if (_screenRoot.CurrentResolution.Equals(resolution))
                    currentIndex = optionDatas.Count - 1;
            }

            _screenDropdown.AddOptions(optionDatas);
            _screenDropdown.value = currentIndex;
        }

        private void OnScreenDropdownValueChanged(int value)
        {
            string variant = _screenDropdown.options[value].text;
            _screenRoot.SetResolution(_screenRoot.GetResolutionData(variant));
        }

        private async UniTask ActivatingGoBackOnMainPanelButton()
        {
            // Ждем 1 кадр тк при нажатии Enter, потом по графику работы методов Unity в кадре срабатывает
            // выделенный Select. Чтоб такого не было надо либо отменить срабатывает Enter-a, либо подождать кадр перед активации Select-a
            // этой кнопки, т.к ничего не сработает, т.к. ничего еще не выделено 
            await UniTask.NextFrame();

            _goBackOnMainPanelButton.Select();
        }

        //private IEnumerator ActivatingGoBackOnMainPanelButton()
        //{
        //    // Ждем 1 кадр тк при нажатии Enter, потом по графику работы методов Unity в кадре срабатывает
        //    // выделенный Select. Чтоб такого не было надо либо отменить срабатывает Enter-a, либо подождать кадр перед активации Select-a
        //    // этой кнопки, т.к ничего не сработает, т.к. ничего еще не выделено 
        //    yield return null;

        //    _goBackOnMainPanelButton.Select();
        //}


        public override void Deactivate()
        {
            if (IsActive == false)
                return;

            base.Deactivate();

            _screenDropdown.options.Clear();
            _goBackOnMainPanelButton.OnDeselect(new BaseEventData(EventSystem.current));
        }

        // Да, у меня 3 пустых метода. Да, получается хуйня. Но так надо для синхронизации New InputSystem-а с Input-ом тут,
        // который уже реализован. А создавать свои слайдеры и дропдауны, я не настолько ебнулся
        public override void OnEnterPress()
        { }

        public override void OnDownArrow()
        { }

        public override void OnUpArrow()
        { }

        public override void OnLeftArrow()
        { }

        public override void OnRightArrow()
        { }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(MenuSettingsButtonsPanel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineGoBackOnMainPanelButton(),
            };

            return list;
        }

        [ContextMenu(nameof(DefineGoBackOnMainPanelButton))]
        private ComponentAttachInfo DefineGoBackOnMainPanelButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _goBackOnMainPanelButton, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}