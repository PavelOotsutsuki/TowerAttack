using System;
using GameFields.FightMenues;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace GameFields.InputSettings
{
    public class InputRoot_OLD// : MonoBehaviour
    {
        private InputType _inputType;

        private bool _isFightMenu;
        private bool _isEnable;

        private readonly InputActions _inputActions;
        private readonly IDeactivatable _endTurnButtonDeactivatable;
        private readonly IPointerClickHandler _choiceSelectModeButtonActivator;
        private readonly IPointerClickHandler _attackSelectModeButtonActivator;
        private readonly IPointerClickHandler _choiceSelectButtonClick;
        private readonly IPointerClickHandler _attackSelectButtonClick;
        private readonly IPointerClickHandler _lookCardMenuButtonClick;
        private readonly IPointerClickHandler _lookCardMenuLeftSwitch;
        private readonly IPointerClickHandler _lookCardMenuRightSwitch;
        private readonly IWorkable _fightMenu;
        private readonly IFocusedButtonEnterHandler _fightMenuButtonsPanel;

        public InputRoot_OLD(IDeactivatable endTurnButtonDeactivatable, IPointerClickHandler choiceSelectModeButtonActivator,
            IPointerClickHandler attackSelectModeButtonActivator, IPointerClickHandler choiceSelectButtonClick,
            IPointerClickHandler attackSelectButtonClick, IPointerClickHandler lookCardMenuButtonClick,
            IPointerClickHandler lookCardMenuLeftSwitch, IPointerClickHandler lookCardMenuRightSwitch,
            IWorkable fightMenu, IFocusedButtonEnterHandler fightMenuButtonsPanel)
        {
            _endTurnButtonDeactivatable = endTurnButtonDeactivatable;
            _choiceSelectModeButtonActivator = choiceSelectModeButtonActivator;
            _attackSelectModeButtonActivator = attackSelectModeButtonActivator;
            _choiceSelectButtonClick = choiceSelectButtonClick;
            _attackSelectButtonClick = attackSelectButtonClick;
            _lookCardMenuButtonClick = lookCardMenuButtonClick;
            _lookCardMenuLeftSwitch = lookCardMenuLeftSwitch;
            _lookCardMenuRightSwitch = lookCardMenuRightSwitch;
            _fightMenu = fightMenu;
            _fightMenuButtonsPanel = fightMenuButtonsPanel;

            Disable();
            _isFightMenu = false;

            _inputActions = new InputActions();

            _inputActions.GameField.Enter.performed += OnEnter;
            _inputActions.GameField.Esc.performed += OnEsc;
            _inputActions.GameField.Q.performed += OnQ;
            _inputActions.GameField.LeftArrow.performed += OnLeftArrow;
            _inputActions.GameField.RightArrow.performed += OnRightArrow;
            _inputActions.GameField.DownArrow.performed += OnDownArrow;
            _inputActions.GameField.UpArrow.performed += OnUpArrow;

            _inputActions.Enable();
        }

        ~InputRoot_OLD()
        {
            _inputActions.GameField.Enter.performed -= OnEnter;
            _inputActions.GameField.Esc.performed -= OnEsc;
            _inputActions.GameField.Q.performed -= OnQ;
            _inputActions.GameField.LeftArrow.performed -= OnLeftArrow;
            _inputActions.GameField.RightArrow.performed -= OnRightArrow;
            _inputActions.GameField.DownArrow.performed -= OnDownArrow;
            _inputActions.GameField.UpArrow.performed -= OnUpArrow;

            _inputActions?.Disable();
        }

        public void Disable()
        {
            _isEnable = false;
        }

        public void ActivateFightMenu()
        {
            _isFightMenu = true;
            Enable();
        }

        public void DeactivateFightMenu()
        {
            _isFightMenu = false;
            Enable();
        }

        public void SetInputType(InputType inputType)
        {
            _inputType = inputType;
            Enable();
        }

        private void Enable()
        {
            _isEnable = true;
        }

        private void OnEnter(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            Debug.Log($"Enter pressed!: {_inputType}");

            if (_isFightMenu)
            {
                OnEnterFightMenu();
                return;
            }

            switch (_inputType)
            {
                case InputType.EndTurnButton:
                    OnEnterEndTurnButton();
                    break;
                case InputType.LookCardMenu:
                    OnEnterLookCardMenu();
                    break;
                case InputType.ChoiceMenu:
                    OnEnterChoiceMenu();
                    break;
                case InputType.AttackMenu:
                    OnEnterAttackMenu();
                    break;
                case InputType.FightProcessing:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnEsc(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            Debug.Log($"Esc pressed!: {_inputType}");

            if (_isFightMenu)
            {
                CloseFightMenu();
                return;
            }

            OpenFightMenu();
        }

        private void OnQ(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            Debug.Log($"Q pressed!: {_inputType}");

            if (_isFightMenu)
            {
                return;
            }

            switch (_inputType)
            {
                case InputType.ChoiceMenu:
                    OnQChoiceMenu();
                    break;
                case InputType.AttackMenu:
                    OnQAttackMenu();
                    break;
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.LookCardMenu:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnLeftArrow(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            Debug.Log($"LeftArrow pressed!: {_inputType}");

            if (_isFightMenu)
            {
                return;
            }

            switch (_inputType)
            {
                case InputType.LookCardMenu:
                    OnLeftArrowLookCardMenu();
                    break;
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnRightArrow(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            Debug.Log($"RightArrow pressed!: {_inputType}");

            if (_isFightMenu)
            {
                return;
            }

            switch (_inputType)
            {
                case InputType.LookCardMenu:
                    OnRightArrowLookCardMenu();
                    break;
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnDownArrow(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            Debug.Log($"DownArrow pressed!: {_inputType}");

            if (_isFightMenu)
            {
                OnDownArrowFightMenu();
                return;
            }

            switch (_inputType)
            {
                case InputType.LookCardMenu:
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnUpArrow(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            Debug.Log($"UpArrow pressed!: {_inputType}");

            if (_isFightMenu)
            {
                OnUpArrowFightMenu();
                return;
            }

            switch (_inputType)
            {
                case InputType.LookCardMenu:
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnEnterEndTurnButton()
        {
            _endTurnButtonDeactivatable.Deactivate();
        }

        private void OpenFightMenu()
        {
            _fightMenu.Activate();
        }

        private void CloseFightMenu()
        {
            _fightMenu.Deactivate();
        }

        private void OnQChoiceMenu()
        {
            _choiceSelectModeButtonActivator.OnPointerClick(null);
        }

        private void OnQAttackMenu()
        {
            _attackSelectModeButtonActivator.OnPointerClick(null);
        }

        private void OnLeftArrowLookCardMenu()
        {
            _lookCardMenuLeftSwitch.OnPointerClick(null);
        }

        private void OnRightArrowLookCardMenu()
        {
            _lookCardMenuRightSwitch.OnPointerClick(null);
        }

        private void OnDownArrowFightMenu()
        {
            _fightMenuButtonsPanel.OnDownArrow();
        }

        private void OnUpArrowFightMenu()
        {
            _fightMenuButtonsPanel.OnUpArrow();
        }

        private void OnEnterFightMenu()
        {
            _fightMenuButtonsPanel.OnEnterPress();
        }

        private void OnEnterLookCardMenu()
        {
            _lookCardMenuButtonClick.OnPointerClick(null);
        }

        private void OnEnterChoiceMenu()
        {
            _choiceSelectButtonClick.OnPointerClick(null);
        }

        private void OnEnterAttackMenu()
        {
            _attackSelectButtonClick.OnPointerClick(null);
        }
    }
}