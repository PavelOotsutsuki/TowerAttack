using System;
using System.Collections.Generic;
using GameFields.EndTurnButtons;
using GameFields.FightMenues;
using GameFields.Persons.Commons;
using GameFields.Persons.EnemyProcessImitations;
using GameFields.Persons.LookCardMenues;
using GameFields.Persons.SelectMenues.Commons;
using Tools;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace GameFields.InputSettings
{
    public class InputRoot : IDeactivatable
    {
        private bool _isFightMenu;
        private bool _isEnable;

        private readonly InputActions _inputActions;
        private readonly IDeactivatable _endTurnButtonDeactivatable;
        private readonly IWorkable _fightMenu;
        private readonly Dictionary<IInputLogicObject, IInputLogic> _logics;

        private readonly FightMenuInputLogic _fightMenuInputLogic;

        private IInputLogic _currentLogic;

        public InputRoot(IDeactivatable endTurnButtonDeactivatable, IWorkable fightMenu,
            IFightMenuInputActivateWatcher fightMenuInputActivateWatcher)
        {
            _fightMenuInputLogic = new FightMenuInputLogic(fightMenuInputActivateWatcher);
            _fightMenu = fightMenu;
            _logics = new Dictionary<IInputLogicObject, IInputLogic>();
            _endTurnButtonDeactivatable = endTurnButtonDeactivatable;

            _isFightMenu = false;
            _currentLogic = null;

            _inputActions = new InputActions();

            _inputActions.GameField.Esc.performed += OnEsc;

            _inputActions.Enable();
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

            _inputActions?.Disable();
        }

        void IDeactivatable.Deactivate()
        {
            _inputActions?.Disable();
        }

        public void Pause()
        {
            _isEnable = false;
        }

        public void ActivateFightMenu()
        {
            _isFightMenu = true;
            SetSubscribes();
            Unpause();
        }

        public void DeactivateFightMenu()
        {
            _isFightMenu = false;
            SetSubscribes();
            Unpause();
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
            Unpause();
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

        private void Unpause()
        {
            _isEnable = true;
        }

        private void OnEnter(CallbackContext context)
        {
            Debug.Log($"Enter pressed!: {_currentLogic}");

            if (_isFightMenu)
            {
                _fightMenuInputLogic.OnEnter();
                return;
            }

            if (_isEnable == false)
                return;

            if (_currentLogic is IEnterPressHandler enterPressHandler)
                enterPressHandler.OnEnter();
        }

        private void OnEsc(CallbackContext context)
        {
            Debug.Log($"Esc pressed!: {_currentLogic}");

            if (_isFightMenu)
            {
                _fightMenu.Deactivate();
                return;
            }

            if (_isEnable == false)
                return;

            _fightMenu.Activate();
        }

        private void OnQ(CallbackContext context)
        {
            Debug.Log($"Q pressed!: {_currentLogic}");

            if (_isEnable == false)
                return;

            if (_currentLogic is IQPressHandler qPressHandler)
                qPressHandler.OnQ();
        }

        private void OnLeftArrow(CallbackContext context)
        {
            Debug.Log($"LeftArrow pressed!: {_currentLogic}");

            if (_isEnable == false)
                return;

            if (_currentLogic is ILeftArrowPressHandler leftArrowPressHandler)
                leftArrowPressHandler.OnLeftArrow();
        }

        private void OnRightArrow(CallbackContext context)
        {
            Debug.Log($"RightArrow pressed!: {_currentLogic}");

            if (_isEnable == false)
                return;

            if (_currentLogic is IRightArrowPressHandler rightArrowPressHandler)
                rightArrowPressHandler.OnRightArrow();
        }

        private void OnDownArrow(CallbackContext context)
        {
            Debug.Log($"DownArrow pressed!: {_currentLogic}");

            if (_isFightMenu)
            {
                _fightMenuInputLogic.OnDownArrow();
                return;
            }

            if (_isEnable == false)
                return;

            if (_currentLogic is IDownArrowPressHandler downArrowPressHandler)
                downArrowPressHandler.OnDownArrow();
        }

        private void OnUpArrow(CallbackContext context)
        {
            Debug.Log($"UpArrow pressed!: {_currentLogic}");

            if (_isFightMenu)
            {
                _fightMenuInputLogic.OnUpArrow();
                return;
            }

            if (_isEnable == false)
                return;

            if (_currentLogic is IUpArrowPressHandler upArrowPressHandler)
                upArrowPressHandler.OnUpArrow();
        }
    }
}