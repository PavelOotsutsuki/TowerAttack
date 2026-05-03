using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Tools.InputSettings;
using Tools.UI;
using Tools.UI.Extendeds;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace StartMenues.LogInButtonsPanels
{
    public class LogInButtonsPanelSelectHandler: IFocusCustomButtonWatcher, IFocusUnityButtonWatcher, IFocusedButtonEnterHandler
    {
        private readonly ExtendedTMP_InputField _loginIF;
        private readonly ExtendedTMP_InputField _passwordIF;
        private readonly ConfirmableFocusableButton _logInButton;
        private readonly ConfirmableFocusableButton _registraitionButton;
        private readonly ConfirmableFocusableButton _exitButton;

        private readonly Dictionary<object, NextFocusData> _allFocusable;
        private object _currentFocused;

        public LogInButtonsPanelSelectHandler(ExtendedTMP_InputField loginIF, ExtendedTMP_InputField passwordIF, ConfirmableFocusableButton logInButton,
            ConfirmableFocusableButton registraitionButton, ConfirmableFocusableButton exitButton)
        {
            _loginIF = loginIF;
            _passwordIF = passwordIF;
            _logInButton = logInButton;
            _registraitionButton = registraitionButton;
            _exitButton = exitButton;

            _loginIF.OnPointerClickEvent -= SetFocused;
            _loginIF.OnPointerClickEvent += SetFocused;

            _passwordIF.OnPointerClickEvent -= SetFocused;
            _passwordIF.OnPointerClickEvent += SetFocused;

            _allFocusable = new Dictionary<object, NextFocusData>
            {
                { _loginIF, null} ,
                { _passwordIF, null} ,
                { _logInButton, new NextFocusData(_registraitionButton, _loginIF, _registraitionButton, _exitButton) } ,
                { _registraitionButton, new NextFocusData(_loginIF, _logInButton, null, _exitButton) } ,
                { _exitButton, new NextFocusData(_loginIF, _logInButton, _registraitionButton, null) } 
            };

            _currentFocused = null;
        }

        ~LogInButtonsPanelSelectHandler()
        {
            _loginIF.OnPointerClickEvent -= SetFocused;
            _passwordIF.OnPointerClickEvent -= SetFocused;
        }

        public void Activate()
        {
            Select(_loginIF);
        }

        public void SetFocused(ConfirmableFocusableButton focused)
        {
            if (_currentFocused == focused)
                return;

            if (_allFocusable.ContainsKey(focused) == false)
                throw new Exception("Ну и какого хера ты пытаешься зафокусить неподвластную тебе ConfirmableFocusableButton???");

            UnfocuseCurrent();

            _currentFocused = focused;

            focused.PointerDisableSettingsRoot.OnPointerExit.Disable();
        }

        public void SetFocused(TMP_InputField inputField)
        {
            if (_currentFocused == inputField)
                return;

            if (_allFocusable.ContainsKey(inputField) == false)
                throw new Exception("Ну и какого хера ты пытаешься зафокусить неподвластную тебе TMP_InputField???");

            UnfocuseCurrent();

            _currentFocused = inputField;
        }

        public void OnEnterPress()
        {
            if (_currentFocused == null)
                return;

            switch (_currentFocused)
            {
                case TMP_InputField:
                    if (_currentFocused == _loginIF)
                    {
                        Select(_passwordIF);
                        break;
                    }

                    if (_currentFocused == _passwordIF)
                    {
                        //_logInButton.OnPointerEnter(new PointerEventData(EventSystem.current));
                        Select(_logInButton);
                        break;
                    }
                    break;
                case ConfirmableFocusableButton confirmableFocusableButton:
                    confirmableFocusableButton.OnPointerClick(null);
                    break;
                default:
                    throw new Exception("Нажимаешь Enter на что-то непонятное");
            }
        }

        public void OnDownArrow()
        {
            OnArrow(InputSideType.OnDown);
        }

        public void OnUpArrow()
        {
            OnArrow(InputSideType.OnUp);
        }

        public void OnLeftArrow()
        {
            OnArrow(InputSideType.OnLeft);
        }

        public void OnRightArrow()
        {
            OnArrow(InputSideType.OnRight);
        }

        private void OnArrow(InputSideType inputSideType)
        {
            if (_currentFocused == null)
                return;

            if (_allFocusable.ContainsKey(_currentFocused) == false)
                throw new Exception("Ну и какого хера у тебя _currentFocusedButton вне словаря???");

            NextFocusData nextFocusData = _allFocusable[_currentFocused];

            if (nextFocusData == null)
                return;

            object OnDown = nextFocusData.GetSide(inputSideType);

            if (OnDown == null)
                return;

            UnfocuseCurrent();
            Select(OnDown);
        }


        private void Select(object selectableObject)
        {
            switch (selectableObject)
            {
                case TMP_InputField inputField:
                    inputField.OnPointerClick(new PointerEventData(EventSystem.current));
                    break;
                case ConfirmableFocusableButton confirmableFocusableButton:
                    confirmableFocusableButton.OnPointerEnter(null);
                    break;
                default:
                    throw new Exception("Select неизвестного типа: " + selectableObject.GetType());
            }

            _currentFocused = selectableObject;
        }


        private void UnfocuseCurrent()
        {
            if (_currentFocused != null)
            {
                switch (_currentFocused)
                {
                    case TMP_InputField TMP_InputField:
                        TMP_InputField.DeactivateInputField();
                        break;
                    case ConfirmableFocusableButton focusable:
                        focusable.PointerDisableSettingsRoot.OnPointerExit.Enable();
                        focusable.OnPointerExit(null);
                        break;
                    default:
                        throw new Exception("Переданный Focusable не TMP_InputField и не ConfirmableFocusableButton: " + _currentFocused.GetType());
                }

                _currentFocused = null;
            }
        }
    }
}