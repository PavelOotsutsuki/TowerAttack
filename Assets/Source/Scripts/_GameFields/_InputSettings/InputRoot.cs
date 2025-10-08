using System;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace GameFields.InputSettings
{
    public class InputRoot : MonoBehaviour
    {
        private InputActions _inputActions;
        private InputType _inputType;

        private IDeactivatable _endTurnButtonDeactivatable;
        private IPointerClickHandler _choiceSelectModeButtonActivator;
        private IPointerClickHandler _attackSelectModeButtonActivator;
        private IPointerClickHandler _choiceSelectButtonClick;
        private IPointerClickHandler _attackSelectButtonClick;

        public void Init(IDeactivatable endTurnButtonDeactivatable, IPointerClickHandler choiceSelectModeButtonActivator,
            IPointerClickHandler attackSelectModeButtonActivator, IPointerClickHandler choiceSelectButtonClick,
            IPointerClickHandler attackSelectButtonClick)
        {
            _endTurnButtonDeactivatable = endTurnButtonDeactivatable;
            _choiceSelectModeButtonActivator = choiceSelectModeButtonActivator;
            _attackSelectModeButtonActivator = attackSelectModeButtonActivator;
            _choiceSelectButtonClick = choiceSelectButtonClick;
            _attackSelectButtonClick = attackSelectButtonClick;

            _inputActions = new InputActions();

            _inputType = InputType.None;

            _inputActions.GameField.Enter.performed += OnEnter;
            _inputActions.GameField.Esc.performed += OnEsc;
            _inputActions.GameField.Q.performed += OnQ;
            _inputActions.GameField.LeftArrow.performed += OnLeftArrow;
            _inputActions.GameField.RightArrow.performed += OnRightArrow;
            _inputActions.GameField.DownArrow.performed += OnDownArrow;
            _inputActions.GameField.UpArrow.performed += OnUpArrow;

            _inputActions.Enable();
        }

        public void SetInputType(InputType inputType)
        {
            _inputType = inputType;
        }

        private void OnDisable()
        {
            _inputActions?.Disable();
        }

        private void OnEnter(CallbackContext context)
        {
            Debug.Log($"Enter pressed!: {_inputType}");

            switch (_inputType)
            {
                case InputType.EndTurnButton:
                    OnEnterEndTurnButton();
                    break;
                case InputType.FightMenu:
                    OnEnterFightMenu();
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
                case InputType.None:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnEsc(CallbackContext context)
        {
            Debug.Log($"Esc pressed!: {_inputType}");

            switch (_inputType)
            {
                case InputType.FightMenu:
                    CloseFightMenu();
                    break;
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.LookCardMenu:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                    OpenFightMenu();
                    break;
                case InputType.None:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnQ(CallbackContext context)
        {
            Debug.Log($"Q pressed!: {_inputType}");

            switch (_inputType)
            {
                case InputType.ChoiceMenu:
                    OnQChoiceMenu();
                    break;
                case InputType.AttackMenu:
                    OnQAttackMenu();
                    break;
                case InputType.FightMenu:
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.LookCardMenu:
                case InputType.None:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnLeftArrow(CallbackContext context)
        {
            Debug.Log($"LeftArrow pressed!: {_inputType}");

            switch (_inputType)
            {
                case InputType.LookCardMenu:
                    OnLeftArrowLookCardMenu();
                    break;
                case InputType.FightMenu:
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                case InputType.None:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnRightArrow(CallbackContext context)
        {
            Debug.Log($"RightArrow pressed!: {_inputType}");

            switch (_inputType)
            {
                case InputType.LookCardMenu:
                    OnRightArrowLookCardMenu();
                    break;
                case InputType.FightMenu:
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                case InputType.None:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnDownArrow(CallbackContext context)
        {
            Debug.Log($"DownArrow pressed!: {_inputType}");

            switch (_inputType)
            {
                case InputType.FightMenu:
                    OnDownArrowFightMenu();
                    break;
                case InputType.LookCardMenu:
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                case InputType.None:
                    break;
                default:
                    throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            }
        }

        private void OnUpArrow(CallbackContext context)
        {
            Debug.Log($"UpArrow pressed!: {_inputType}");

            switch (_inputType)
            {
                case InputType.FightMenu:
                    OnUpArrowFightMenu();
                    break;
                case InputType.LookCardMenu:
                case InputType.EndTurnButton:
                case InputType.FightProcessing:
                case InputType.ChoiceMenu:
                case InputType.AttackMenu:
                case InputType.None:
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

        }

        private void CloseFightMenu()
        {

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

        }

        private void OnRightArrowLookCardMenu()
        {

        }

        private void OnDownArrowFightMenu()
        {

        }

        private void OnUpArrowFightMenu()
        {

        }

        private void OnEnterFightMenu()
        {

        }

        private void OnEnterLookCardMenu()
        {

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