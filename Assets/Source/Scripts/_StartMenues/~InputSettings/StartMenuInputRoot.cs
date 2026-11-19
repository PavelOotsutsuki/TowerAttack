using System;
using System.Collections.Generic;
using Menues;
using Tools;
using Tools.InputSettings;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

namespace StartMenues.InputSettings
{
    public class StartMenuInputRoot : IWorkable
    {
        //private bool _isFightMenu;
        private bool _isEnable;

        private readonly InputActions _inputActions;
        //private readonly Dictionary<IInputLogicObject, IInputLogic> _logics;

        private readonly MenuInputLogic _startMenuInputLogic;

        //private IInputLogic _currentLogic;

        public bool? IsActive { get; private set; } = null;

        public StartMenuInputRoot(IMenuInputActivateWatcher startMenuInputActivateWatcher)
        {
            _startMenuInputLogic = new MenuInputLogic(startMenuInputActivateWatcher);
            //_logics = new Dictionary<IInputLogicObject, IInputLogic>();

            //_isFightMenu = false;
            //_currentLogic = null;

            _inputActions = new InputActions();

            //_inputActions.Enable();
            _inputActions.StartMenu.Enter.performed += OnEnter;
            //_inputActions.GameField.Q.performed -= OnQ;
            _inputActions.StartMenu.LeftArrow.performed += OnLeftArrow;
            _inputActions.StartMenu.RightArrow.performed += OnRightArrow;
            _inputActions.StartMenu.DownArrow.performed += OnDownArrow;
            _inputActions.StartMenu.UpArrow.performed += OnUpArrow;
            //SetSubscribes();
            Deactivate();
        }

        ~StartMenuInputRoot()
        {
            _inputActions.StartMenu.Enter.performed -= OnEnter;
            _inputActions.StartMenu.LeftArrow.performed -= OnLeftArrow;
            _inputActions.StartMenu.RightArrow.performed -= OnRightArrow;
            _inputActions.StartMenu.DownArrow.performed -= OnDownArrow;
            _inputActions.StartMenu.UpArrow.performed -= OnUpArrow;

            _inputActions?.Disable();
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _inputActions?.Enable();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _inputActions?.Disable();
        }

        public void Pause()
        {
            _isEnable = false;
        }

        //public void ActivateFightMenu()
        //{
        //    _isFightMenu = true;
        //    SetSubscribes();
        //    Unpause();
        //}

        //public void DeactivateFightMenu()
        //{
        //    _isFightMenu = false;
        //    SetSubscribes();
        //    Unpause();
        //}

        //public void SetInputType(IInputLogicObject inputLogicObject)
        //{
        //    ClearLogics();

        //    if (_logics.ContainsKey(inputLogicObject))
        //    {
        //        _currentLogic = _logics[inputLogicObject];
        //    }
        //    else
        //    {
        //        _currentLogic = inputLogicObject switch
        //        {
        //            EndTurnProcessing => new EndTurnButtonInputLogic(_endTurnButtonDeactivatable),
        //            SelectMenuPlayer selectMenuPlayer => new SelectMenuPlayerInputLogic(selectMenuPlayer),
        //            LookCardMenuPlayer lookCardMenuPlayer => new LookCardMenuPlayerInputLogic(lookCardMenuPlayer),
        //            TurnProcessing => new TurnProcessingInputLogic(),
        //            EnemyDragAndDropImitation => new EnemyDragAndDropImitationInputLogic(),
        //            CardActionProcessingEnemyAI => new CardActionProcessingEnemyAIInputLogic(),
        //            OnBeforeEndTurnProcessing => new OnBeforeEndTurnProcessingInputLogic(),
        //            _ => throw new NullReferenceException($"Ошибка задания логики Input-a. Неизвестный IInputLogicObject: {inputLogicObject}")
        //        };

        //        _logics.Add(inputLogicObject, _currentLogic);
        //    }

        //    SetSubscribes();
        //    Unpause();
        //}

        //private void SetSubscribes()
        //{
        //    _inputActions.StartMenu.Enter.performed -= OnEnter;
        //    //_inputActions.GameField.Q.performed -= OnQ;
        //    //_inputActions.GameField.LeftArrow.performed -= OnLeftArrow;
        //    //_inputActions.GameField.RightArrow.performed -= OnRightArrow;
        //    _inputActions.StartMenu.DownArrow.performed -= OnDownArrow;
        //    _inputActions.StartMenu.UpArrow.performed -= OnUpArrow;

        //    if (_currentLogic is IEnterPressHandler)
        //        _inputActions.StartMenu.Enter.performed += OnEnter;

        //    //if (_currentLogic is IQPressHandler)
        //    //    _inputActions.GameField.Q.performed += OnQ;

        //    //if (_currentLogic is ILeftArrowPressHandler)
        //    //    _inputActions.GameField.LeftArrow.performed += OnLeftArrow;

        //    //if (_currentLogic is IRightArrowPressHandler)
        //    //    _inputActions.GameField.RightArrow.performed += OnRightArrow;

        //    if (_currentLogic is IDownArrowPressHandler)
        //        _inputActions.StartMenu.DownArrow.performed += OnDownArrow;

        //    if (_currentLogic is IUpArrowPressHandler)
        //        _inputActions.StartMenu.UpArrow.performed += OnUpArrow;

        //}

        //private void ClearLogics()
        //{
        //    List<IInputLogicObject> deletableKeys = new List<IInputLogicObject>();

        //    foreach (IInputLogicObject logic in _logics.Keys)
        //    {
        //        if (logic == null)
        //        {
        //            Debug.Log($"_logics содержит null-s: {logic}");
        //            deletableKeys.Add(logic);
        //        }
        //    }

        //    if (deletableKeys.Count > 0)
        //    {
        //        foreach (IInputLogicObject logic in deletableKeys)
        //        {
        //            _logics.Remove(logic);
        //        }
        //    }
        //}

        public void Unpause()
        {
            _isEnable = true;
        }

        private void OnEnter(CallbackContext context)
        {
            //Debug.Log($"Enter pressed!: {_currentLogic}");
            if (_isEnable == false)
                return;

            //if (_isFightMenu)
            //{
                _startMenuInputLogic.OnEnter();
                return;
            //}

            //if (_isEnable == false)
            //    return;

            //if (_currentLogic is IEnterPressHandler enterPressHandler)
            //    enterPressHandler.OnEnter();
        }

        //private void OnEsc(CallbackContext context)
        //{
        //    //Debug.Log($"Esc pressed!: {_currentLogic}");

        //    if (_isFightMenu)
        //    {
        //        _fightMenu.Deactivate();
        //        return;
        //    }

        //    if (_isEnable == false)
        //        return;

        //    _fightMenu.Activate();
        //}

        //private void OnH(CallbackContext context)
        //{
        //    //Debug.Log($"H pressed!: {_currentLogic}");

        //    //if (_historyMenu.IsActive == true)
        //    //{
        //    //    _historyMenu.Deactivate();
        //    //}
        //    //else
        //    //{
        //    //    _historyMenu.Activate();
        //    //}

        //    if (_historyMenu.IsActive == true)
        //    {
        //        _historyMenu.Deactivate();
        //        return;
        //    }

        //    if (_isEnable == false)
        //        return;

        //    _historyMenu.Activate();
        //}

        //private void OnQ(CallbackContext context)
        //{
        //    //Debug.Log($"Q pressed!: {_currentLogic}");

        //    if (_isFightMenu)
        //        return;

        //    if (_isEnable == false)
        //        return;

        //    if (_currentLogic is IQPressHandler qPressHandler)
        //        qPressHandler.OnQ();
        //}

        private void OnLeftArrow(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            _startMenuInputLogic.OnLeftArrow();
                return;

            //Debug.Log($"LeftArrow pressed!: {_currentLogic}");

            //if (_isFightMenu)
            //    return;

            //if (_isEnable == false)
            //    return;

            //if (_currentLogic is ILeftArrowPressHandler leftArrowPressHandler)
            //    leftArrowPressHandler.OnLeftArrow();
        }

        private void OnRightArrow(CallbackContext context)
        {
            if (_isEnable == false)
                return;

            _startMenuInputLogic.OnRightArrow();
                return;

            //Debug.Log($"RightArrow pressed!: {_currentLogic}");

            //if (_isFightMenu)
            //    return;

            //if (_isEnable == false)
            //    return;

            //if (_currentLogic is IRightArrowPressHandler rightArrowPressHandler)
            //    rightArrowPressHandler.OnRightArrow();
        }

        private void OnDownArrow(CallbackContext context)
        {
            //Debug.Log($"DownArrow pressed!: {_currentLogic}");
            if (_isEnable == false)
                return;
            //if (_isFightMenu)
            //{
            _startMenuInputLogic.OnDownArrow();
                return;
            //}

            //if (_isEnable == false)
            //    return;

            //if (_currentLogic is IDownArrowPressHandler downArrowPressHandler)
            //    downArrowPressHandler.OnDownArrow();
        }

        private void OnUpArrow(CallbackContext context)
        {
            //Debug.Log($"UpArrow pressed!: {_currentLogic}");
            if (_isEnable == false)
                return;
            //if (_isFightMenu)
            //{
            _startMenuInputLogic.OnUpArrow();
                return;
            //}

            //if (_isEnable == false)
            //    return;

            //if (_currentLogic is IUpArrowPressHandler upArrowPressHandler)
            //    upArrowPressHandler.OnUpArrow();
        }
    }
}