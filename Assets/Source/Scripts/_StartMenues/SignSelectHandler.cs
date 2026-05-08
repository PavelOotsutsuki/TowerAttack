using System;
using System.Collections.Generic;
using System.Linq;
using Tools.InputSettings;
using Tools.UI;
using Tools.UI.Extendeds;
using UnityEngine.EventSystems;
using ISelectHandler = Menues.ISelectHandler;

namespace StartMenues
{
    public abstract class SignSelectHandler : ISelectHandler
    {
        private readonly List<ExtendedTMP_InputField> _subscribed;
        private readonly IReadOnlyDictionary<object, NextFocusData> _allFocusableDictionary;
        private readonly List<object> _allFocusableList;

        private object _currentFocused;

        public SignSelectHandler(IReadOnlyDictionary<object, NextFocusData> allFocusableDictionary)
        {
            _subscribed = new List<ExtendedTMP_InputField>();
            _allFocusableList = new List<object>();

            _allFocusableDictionary = allFocusableDictionary;

            foreach (object focusable in allFocusableDictionary.Keys)
            {
                if (focusable is ExtendedTMP_InputField inputField)
                    SubscribeFocused(inputField);

                _allFocusableList.Add(focusable);
            }

            _currentFocused = null;
        }

        ~SignSelectHandler()
        {
            UnsubscribeFocused();
        }

        public void Activate()
        {
            Select(_allFocusableList[0]);
        }

        public void OnEnterPress()
        {
            if (_currentFocused == null)
                return;

            switch (_currentFocused)
            {
                case ExtendedTMP_InputField:

                    int index = _allFocusableList.IndexOf(_currentFocused);

                    index++;

                    if (index >= _allFocusableList.Count)
                        index = 0;

                    Select(_allFocusableList[index]);
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

        public void SetFocused(ConfirmableFocusableButton focused)
        {
            if (_currentFocused == focused)
                return;

            if (_allFocusableDictionary.ContainsKey(focused) == false)
                throw new Exception("Ну и какого хера ты пытаешься зафокусить неподвластную тебе ConfirmableFocusableButton???");

            UnfocuseCurrent();

            _currentFocused = focused;

            focused.PointerDisableSettingsRoot.OnPointerExit.Disable();
        }

        public void SetFocused(ExtendedTMP_InputField inputField)
        {
            if (_currentFocused == inputField)
                return;

            if (_allFocusableDictionary.ContainsKey(inputField) == false)
                throw new Exception("Ну и какого хера ты пытаешься зафокусить неподвластную тебе TMP_InputField???");

            UnfocuseCurrent();

            _currentFocused = inputField;
        }

        private void OnArrow(InputSideType inputSideType)
        {
            if (_currentFocused == null)
                return;

            if (_allFocusableDictionary.ContainsKey(_currentFocused) == false)
                throw new Exception("Ну и какого хера у тебя _currentFocusedButton вне словаря???");

            NextFocusData nextFocusData = _allFocusableDictionary[_currentFocused];

            if (nextFocusData == null)
                return;

            object OnSide = nextFocusData.GetSide(inputSideType);

            if (OnSide == null)
                return;

            Select(OnSide);
        }

        private void Select(object selectableObject)
        {
            UnfocuseCurrent();

            switch (selectableObject)
            {
                case ExtendedTMP_InputField inputField:
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
            EventSystem.current.SetSelectedGameObject(null);

            if (_currentFocused != null)
            {
                switch (_currentFocused)
                {
                    case ExtendedTMP_InputField TMP_InputField:
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

            foreach (ExtendedTMP_InputField focusableKey in _allFocusableList.Where(f => f is ExtendedTMP_InputField))
            {
                focusableKey.DeactivateInputField();
            }
        }

        private void SubscribeFocused(ExtendedTMP_InputField inputField)
        {
            inputField.OnPointerClickEvent -= SetFocused;
            inputField.OnPointerClickEvent += SetFocused;

            if (_subscribed.Contains(inputField) == false)
                _subscribed.Add(inputField);
        }

        private void UnsubscribeFocused()
        {
            foreach (ExtendedTMP_InputField inputField in _subscribed)
                inputField.OnPointerClickEvent -= SetFocused;

            _subscribed.Clear();
        }
    }
}