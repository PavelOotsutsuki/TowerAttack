using System;
using System.Collections.Generic;
using System.Linq;
using Cards;
using GameFields.Effects;
using GameFields.EndTurnButtons;
using GameFields.FightMenues;
using GameFields.Persons.Commons;
using GameFields.Persons.EnemyProcessImitations;
using GameFields.Persons.LookCardMenues;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.InputSystem.InputAction;

namespace GameFields.InputSettings
{
    public class InputRoot// : MonoBehaviour
    {
        //private InputType _inputType;

        private bool _isFightMenu;
        //private bool _isEnable;

        private readonly InputActions _inputActions;
        private readonly IDeactivatable _endTurnButtonDeactivatable;
        //private readonly IPointerClickHandler _choiceSelectModeButtonActivator;
        //private readonly IPointerClickHandler _attackSelectModeButtonActivator;
        //private readonly IPointerClickHandler _choiceSelectButtonClick;
        //private readonly IPointerClickHandler _attackSelectButtonClick;
        //private readonly IPointerClickHandler _lookCardMenuButtonClick;
        //private readonly IPointerClickHandler _lookCardMenuLeftSwitch;
        //private readonly IPointerClickHandler _lookCardMenuRightSwitch;
        private readonly IWorkable _fightMenu;
        private readonly Dictionary<IInputLogicObject, IInputLogic> _logics;

        private readonly FightMenuInputLogic _fightMenuInputLogic;

        private IInputLogic _currentLogic;

        //private IEnterPressHandler _enterPressHandler;
        //private IQPressHandler _qPressHandler;
        //private ILeftArrowPressHandler _leftArrowPressHandler;
        //private IRightArrowPressHandler _rightArrowPressHandler;
        //private IDownArrowPressHandler _downArrowPressHandler;
        //private IUpArrowPressHandler _upArrowPressHandler;

        public InputRoot(IDeactivatable endTurnButtonDeactivatable, IWorkable fightMenu,
            IFightMenuInputActivateWatcher fightMenuInputActivateWatcher)
        {
            _fightMenuInputLogic = new FightMenuInputLogic(fightMenuInputActivateWatcher);
            _fightMenu = fightMenu;
            _logics = new Dictionary<IInputLogicObject, IInputLogic>();
            _endTurnButtonDeactivatable = endTurnButtonDeactivatable;
            //EndTurnButtonInputLogic endTurnButtonInputLogic = new EndTurnButtonInputLogic(endTurnButtonDeactivatable);
            //_logics.Add(endTurnProcessing, endTurnButtonInputLogic);
            //Disable();
            _isFightMenu = false;
            _currentLogic = null;

            _inputActions = new InputActions();

            _inputActions.GameField.Enter.performed += OnEnter;
            _inputActions.GameField.Esc.performed += OnEsc;
            _inputActions.GameField.Q.performed += OnQ;
            _inputActions.GameField.LeftArrow.performed += OnLeftArrow;
            _inputActions.GameField.RightArrow.performed += OnRightArrow;
            _inputActions.GameField.DownArrow.performed += OnDownArrow;
            _inputActions.GameField.UpArrow.performed += OnUpArrow;

            //_inputActions.Enable();
            Disable();
        }

        ~InputRoot()
        {
            _inputActions.GameField.Enter.performed -= OnEnter;
            _inputActions.GameField.Esc.performed -= OnEsc;
            _inputActions.GameField.Q.performed -= OnQ;
            _inputActions.GameField.LeftArrow.performed -= OnLeftArrow;
            _inputActions.GameField.RightArrow.performed -= OnRightArrow;
            _inputActions.GameField.DownArrow.performed -= OnDownArrow;
            _inputActions.GameField.UpArrow.performed -= OnUpArrow;

            //_inputActions?.Disable();
            Disable();
        }

        public void Disable()
        {
            //_isEnable = false;
            _inputActions?.Disable();
        }

        public void ActivateFightMenu()
        {
            _isFightMenu = true;
            SetSubscribes();
            Enable();
        }

        public void DeactivateFightMenu()
        {
            _isFightMenu = false;
            SetSubscribes();
            Enable();
        }

        public void SetInputType(IInputLogicObject inputLogicObject)
        {
            ClearLogics();

            if (_logics.ContainsKey(inputLogicObject))
            {
                _currentLogic = _logics[inputLogicObject];
            }
            else
            {
                _currentLogic = inputLogicObject switch
                {
                    EndTurnProcessing => new EndTurnButtonInputLogic(_endTurnButtonDeactivatable),
                    SelectMenuPlayer selectMenuPlayer => new SelectMenuPlayerInputLogic(selectMenuPlayer),
                    LookCardMenuPlayer lookCardMenuPlayer => new LookCardMenuPlayerInputLogic(lookCardMenuPlayer),
                    TurnProcessing => new TurnProcessingInputLogic(),
                    EnemyDragAndDropImitation => new EnemyDragAndDropImitationInputLogic(),
                    CardActionProcessingEnemyAI => new CardActionProcessingEnemyAIInputLogic(),
                    OnBeforeEndTurnProcessing => new OnBeforeEndTurnProcessingInputLogic(),
                    _ => throw new NullReferenceException($"Ошибка задания логики Input-a. Неизвестный IInputLogicObject: {inputLogicObject}")
                };

                _logics.Add(inputLogicObject, _currentLogic);
            }

            SetSubscribes();
            Enable();
        }

        private void SetSubscribes()
        {
            _inputActions.GameField.Enter.performed -= OnEnter;
            _inputActions.GameField.Q.performed -= OnQ;
            _inputActions.GameField.LeftArrow.performed -= OnLeftArrow;
            _inputActions.GameField.RightArrow.performed -= OnRightArrow;
            _inputActions.GameField.DownArrow.performed -= OnDownArrow;
            _inputActions.GameField.UpArrow.performed -= OnUpArrow;

            if (_currentLogic is IEnterPressHandler || _isFightMenu)
                _inputActions.GameField.Enter.performed += OnEnter;

            if (_currentLogic is IQPressHandler)
                _inputActions.GameField.Q.performed += OnQ;

            if (_currentLogic is ILeftArrowPressHandler)
                _inputActions.GameField.LeftArrow.performed += OnLeftArrow;

            if (_currentLogic is IRightArrowPressHandler)
                _inputActions.GameField.RightArrow.performed += OnRightArrow;

            if (_currentLogic is IDownArrowPressHandler || _isFightMenu)
                _inputActions.GameField.DownArrow.performed += OnDownArrow;

            if (_currentLogic is IUpArrowPressHandler || _isFightMenu)
                _inputActions.GameField.UpArrow.performed += OnUpArrow;

        }

        private void ClearLogics()
        {
            List<IInputLogicObject> deletableKeys = new List<IInputLogicObject>();

            foreach (IInputLogicObject logic in _logics.Keys)
            {
                if (logic == null)
                {
                    Debug.Log($"_logics содержит null-s: {logic}");
                    deletableKeys.Add(logic);
                }
            }

            if (deletableKeys.Count > 0)
            {
                foreach (IInputLogicObject logic in deletableKeys)
                {
                    _logics.Remove(logic);
                }
            }
        }

        private void Enable()
        {
            _inputActions?.Enable();
            //_isEnable = true;
        }

        private void OnEnter(CallbackContext context)
        {
            Debug.Log($"Enter pressed!: {_currentLogic}");

            if (_isFightMenu)
            {
                _fightMenuInputLogic.OnEnter();
                return;
            }

            if (_currentLogic is IEnterPressHandler enterPressHandler)
                enterPressHandler.OnEnter();

            //if (_isEnable == false)
            //    return;


            //if (_isFightMenu)
            //{
            //    OnEnterFightMenu();
            //    return;
            //}

            //switch (_inputType)
            //{
            //    case InputType.EndTurnButton:
            //        OnEnterEndTurnButton();
            //        break;
            //    case InputType.LookCardMenu:
            //        OnEnterLookCardMenu();
            //        break;
            //    case InputType.ChoiceMenu:
            //        OnEnterChoiceMenu();
            //        break;
            //    case InputType.AttackMenu:
            //        OnEnterAttackMenu();
            //        break;
            //    case InputType.FightProcessing:
            //        break;
            //    default:
            //        throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            //}
        }

        private void OnEsc(CallbackContext context)
        {
            //if (_isEnable == false)
            //    return;

            Debug.Log($"Esc pressed!: {_currentLogic}");

            if (_isFightMenu)
            {
                _fightMenu.Deactivate();
                return;
            }

            _fightMenu.Activate();
        }

        private void OnQ(CallbackContext context)
        {
            Debug.Log($"Q pressed!: {_currentLogic}");

            if (_currentLogic is IQPressHandler qPressHandler)
                qPressHandler.OnQ();

            //if (_isEnable == false)
            //    return;

            //Debug.Log($"Q pressed!: {_inputType}");

            //if (_isFightMenu)
            //{
            //    return;
            //}

            //switch (_inputType)
            //{
            //    case InputType.ChoiceMenu:
            //        OnQChoiceMenu();
            //        break;
            //    case InputType.AttackMenu:
            //        OnQAttackMenu();
            //        break;
            //    case InputType.EndTurnButton:
            //    case InputType.FightProcessing:
            //    case InputType.LookCardMenu:
            //        break;
            //    default:
            //        throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            //}
        }

        private void OnLeftArrow(CallbackContext context)
        {
            Debug.Log($"LeftArrow pressed!: {_currentLogic}");

            if (_currentLogic is ILeftArrowPressHandler leftArrowPressHandler)
                leftArrowPressHandler.OnLeftArrow();

            //if (_isEnable == false)
            //    return;

            //Debug.Log($"LeftArrow pressed!: {_inputType}");

            //if (_isFightMenu)
            //{
            //    return;
            //}

            //switch (_inputType)
            //{
            //    case InputType.LookCardMenu:
            //        OnLeftArrowLookCardMenu();
            //        break;
            //    case InputType.EndTurnButton:
            //    case InputType.FightProcessing:
            //    case InputType.ChoiceMenu:
            //    case InputType.AttackMenu:
            //        break;
            //    default:
            //        throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            //}
        }

        private void OnRightArrow(CallbackContext context)
        {
            Debug.Log($"RightArrow pressed!: {_currentLogic}");

            if (_currentLogic is IRightArrowPressHandler rightArrowPressHandler)
                rightArrowPressHandler.OnRightArrow();

            //if (_isEnable == false)
            //    return;

            //Debug.Log($"RightArrow pressed!: {_inputType}");

            //if (_isFightMenu)
            //{
            //    return;
            //}

            //switch (_inputType)
            //{
            //    case InputType.LookCardMenu:
            //        OnRightArrowLookCardMenu();
            //        break;
            //    case InputType.EndTurnButton:
            //    case InputType.FightProcessing:
            //    case InputType.ChoiceMenu:
            //    case InputType.AttackMenu:
            //        break;
            //    default:
            //        throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            //}
        }

        private void OnDownArrow(CallbackContext context)
        {
            Debug.Log($"DownArrow pressed!: {_currentLogic}");

            if (_isFightMenu)
            {
                _fightMenuInputLogic.OnDownArrow();
                return;
            }

            if (_currentLogic is IDownArrowPressHandler downArrowPressHandler)
                downArrowPressHandler.OnDownArrow();

            //if (_isEnable == false)
            //    return;

            //Debug.Log($"DownArrow pressed!: {_inputType}");

            //if (_isFightMenu)
            //{
            //    OnDownArrowFightMenu();
            //    return;
            //}

            //switch (_inputType)
            //{
            //    case InputType.LookCardMenu:
            //    case InputType.EndTurnButton:
            //    case InputType.FightProcessing:
            //    case InputType.ChoiceMenu:
            //    case InputType.AttackMenu:
            //        break;
            //    default:
            //        throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            //}
        }

        private void OnUpArrow(CallbackContext context)
        {
            Debug.Log($"UpArrow pressed!: {_currentLogic}");

            if (_isFightMenu)
            {
                _fightMenuInputLogic.OnUpArrow();
                return;
            }

            if (_currentLogic is IUpArrowPressHandler upArrowPressHandler)
                upArrowPressHandler.OnUpArrow();

            //if (_isEnable == false)
            //    return;

            //Debug.Log($"UpArrow pressed!: {_inputType}");

            //if (_isFightMenu)
            //{
            //    OnUpArrowFightMenu();
            //    return;
            //}

            //switch (_inputType)
            //{
            //    case InputType.LookCardMenu:
            //    case InputType.EndTurnButton:
            //    case InputType.FightProcessing:
            //    case InputType.ChoiceMenu:
            //    case InputType.AttackMenu:
            //        break;
            //    default:
            //        throw new Exception($"Неизвестный InputType: {_inputType}. Класс: {nameof(InputRoot)}");
            //}
        }

        //private void OnEnterEndTurnButton()
        //{
        //    _endTurnButtonDeactivatable.Deactivate();
        //}

        //private void OpenFightMenu()
        //{
        //    _fightMenu.Activate();
        //}

        //private void CloseFightMenu()
        //{
        //    _fightMenu.Deactivate();
        //}

        //private void OnQChoiceMenu()
        //{
        //    _choiceSelectModeButtonActivator.OnPointerClick(null);
        //}

        //private void OnQAttackMenu()
        //{
        //    _attackSelectModeButtonActivator.OnPointerClick(null);
        //}

        //private void OnLeftArrowLookCardMenu()
        //{
        //    _lookCardMenuLeftSwitch.OnPointerClick(null);
        //}

        //private void OnRightArrowLookCardMenu()
        //{
        //    _lookCardMenuRightSwitch.OnPointerClick(null);
        //}

        //private void OnDownArrowFightMenu()
        //{
        //    _fightMenuInputActivateWatcher.OnDownArrow();
        //}

        //private void OnUpArrowFightMenu()
        //{
        //    _fightMenuInputActivateWatcher.OnUpArrow();
        //}

        //private void OnEnterFightMenu()
        //{
        //    _fightMenuInputActivateWatcher.OnEnterPress();
        //}

        //private void OnEnterLookCardMenu()
        //{
        //    _lookCardMenuButtonClick.OnPointerClick(null);
        //}

        //private void OnEnterChoiceMenu()
        //{
        //    _choiceSelectButtonClick.OnPointerClick(null);
        //}

        //private void OnEnterAttackMenu()
        //{
        //    _attackSelectButtonClick.OnPointerClick(null);
        //}
    }
}