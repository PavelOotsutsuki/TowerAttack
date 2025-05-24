using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GameFields.Persons.Common;
using GameFields.Persons.SelectMenues.Choices;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GameFields.Persons.SelectMenues.Commons
{
    [RequireComponent(typeof(FadablePanel))]
    public abstract class SelectNumberPanelPlayer : SelectNumberPanel, ISelectNumberActivator
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private SelectNumber[] _selectNumbers;
        [SerializeField] private SelectNumberPanelPlayerData _data;

        protected List<SelectNumber> CurrentSelectedNumbers;

        private int _columnsCount;
        private int _rowsCount;
        private int _lastRowColumnsCount;
        private float _columnsIndent;
        private float _rowsIndent;
        private float _maxHeight;
        private float _maxWidth;

        private SelectNumberClickHandler _currentSelectNumberClickHandler;

        private IWorkable _selectButton;

        private bool _isCompleteNumbersHide;

        public bool IsCompleteNumbersHide => _isCompleteNumbersHide;

        public void Init(IWorkable selectButton, ICardNumberKeeper cardNumberKeeper, int countNumbers,
            SelectNumbersList selectedNumbers, ConfirmableNumbers confirmableNumbers)
        {
            if (_selectNumbers.Length != countNumbers)
                throw new Exception("Несовпадение заданного кол-ва номеров и кол-ва объектов AttackNumber");

            _selectButton = selectButton;

            base.Init(cardNumberKeeper, countNumbers, selectedNumbers, confirmableNumbers, _selectNumbers);
        }

        public void ActivateNumbers(bool isConfirmableActivate)
        {
            _currentSelectNumberClickHandler.Reset();

            foreach (SelectNumber selectNumber in _selectNumbers)
            {
                selectNumber.Deactivate();
                NumberAnimationType? numberAnimationType = FindActivateType(selectNumber, isConfirmableActivate);

                SelectNumberActivateData data = new SelectNumberActivateData(numberAnimationType, _currentSelectNumberClickHandler);

                selectNumber.Activate(data);

                if (CurrentAvailableNumbers.Contains(selectNumber) == false)
                {
                    selectNumber.Disable();
                }
            }
        }

        protected override void InitNumbers()
        {
            FindColumnsAndRowsCount();
            FindIndents();

            int number = 1;

            foreach (SelectNumber selectNumber in _selectNumbers)
            {
                selectNumber.Init(number, CalcNumberPosition(number), new Vector2(_data.NumberWidht, _data.NumberHeight));
                number++;
            }
        }

        protected override void OnActivate()
        {
            _currentSelectNumberClickHandler = IsConsecutiveMode ?
                new ConsecutiveSelectNumberClickHandler(NeedForActivate, _selectButton, _selectNumbers) :
                new DefaultSelectNumberClickHandler(NeedForActivate, _selectButton);

            ActivateNumbers(true);

            IsCompleteThis = true;
        }

        protected override void OnDeactivate()
        {
            _isCompleteNumbersHide = false;
            _currentSelectNumberClickHandler = null;

            base.OnDeactivate();
        }

        protected override IEnumerator Deactivating()
        {
            CurrentSelectedNumbers = new List<SelectNumber>(); // Можно заменить на LINQ

            foreach (SelectNumber selectNumber in _selectNumbers)
            {
                if (selectNumber.IsClicked)
                {
                    CurrentSelectedNumbers.Add(selectNumber);
                }
            }
            //Debug.Log("Длина: " + _attackNumbers.Length);
            //for (int i = 0; i < _attackNumbers.Length; i++)
            //{
            //    if (_attackNumbers[i].IsClicked)
            //    {
            //        Debug.Log(i+1);
            //        selectedNumbers.Add(_attackNumbers[i]);
            //    }
            //}

            ResultType resultType = ResultType.Falled;

            foreach (SelectNumber selectedNumber in CurrentSelectedNumbers)
            {
                if (CardNumberKeeper.Card.IsSuccessAttack(selectedNumber.Number))
                {
                    //SetChoiceNumber(selectedNumber, ResultType.Success);
                    selectedNumber.SetChoice(ConvertResultTypeToNumberAnimationType(ResultType.Success));

                    resultType = ResultType.Success;
                }
                else
                {
                    //SetChoiceNumber(selectedNumber, ResultType.Falled);
                    selectedNumber.SetChoice(ConvertResultTypeToNumberAnimationType(ResultType.Falled));
                    //selectedNumber.SetChoice(this is ChoiceNumberPanelPlayer ? NumberAnimationType.Choice : NumberAnimationType.Error);
                }

                //SelectedNumbers.Add(selectedNumber);

                float delayUntilPlayNextSelectedNumberAnimation = selectedNumber.AnimationDuration * _data.NextAnimationStartPercent;

                yield return new WaitForSeconds(delayUntilPlayNextSelectedNumberAnimation);
            }

            if (resultType == ResultType.Falled)
                foreach (SelectNumber selectedNumber in CurrentSelectedNumbers)
                {
                    SelectedNumbers.Add(selectedNumber.Number, ConvertResultTypeToNumberAnimationType(resultType));
                }

            SetSelectResultData setSelectResultData = CreateSetSelectResultData(resultType);

            SelectResult.SetResult(setSelectResultData);

            float waitLastAnimationCompleted = CurrentSelectedNumbers[CurrentSelectedNumbers.Count - 1].AnimationDuration * (1f - _data.NextAnimationStartPercent);
            yield return new WaitForSeconds(waitLastAnimationCompleted);
            yield return new WaitForSeconds(_data.DelayAfterAllNumbersAnimationsPlayed / 3f);
            //GC.Collect();
            yield return new WaitForSeconds(_data.DelayAfterAllNumbersAnimationsPlayed * 2f / 3f);

            _isCompleteNumbersHide = true;

            FadablePanel.Hide();

            foreach (SelectNumber selectNumber in _selectNumbers)
            {
                selectNumber.Deactivate();
            }

            yield return new WaitUntil(() => FadablePanel.IsComplete);

            IsCompleteThis = true;
        }

        protected abstract SetSelectResultData CreateSetSelectResultData(ResultType resultType);
        //protected abstract void ActivateNumber(SelectNumber target);
        //protected abstract void SetChoiceNumber(SelectNumber target, ResultType resultType);
        protected abstract NumberAnimationType ConvertResultTypeToNumberAnimationType(ResultType resultType);

        private NumberAnimationType? FindActivateType(SelectNumber selectNumber, bool isConfirmableActivate)
        {
            NumberAnimationType? numberAnimationType = null;

            if (isConfirmableActivate)
            {
                SelectNumbersList fullList = ConfirmableNumbers.FullList;

                if (fullList.Contains(selectNumber.Number))
                {
                    numberAnimationType = fullList.GetType(selectNumber.Number);
                }
            }

            return numberAnimationType;
        }

        private Vector2 CalcNumberPosition(int number)
        {
            if (number < 1 && number > _selectNumbers.Length)
            {
                throw new ArgumentOutOfRangeException($"Такого number-a нет! Number: {number}. MaxLenght: {_selectNumbers.Length}");
            }
            int row = (number - 1) / _columnsCount + 1;
            int column = ((number - 1) % _columnsCount) + 1;
            float x = _data.Indent + _columnsIndent * column + _data.NumberWidht / 2 + _data.NumberWidht * (column - 1) - _maxWidth / 2;
            float y = _data.Indent * (-1) + _maxHeight - (_rowsIndent * row + _data.NumberHeight / 2 + _data.NumberHeight * (row - 1)) - _maxHeight / 2;
            Vector3 position = new Vector2(x, y);
            return position;
        }

        private void FindIndents()
        {
            //ScreenView.GetFactorX();
            //int maxHeight = Screen.height;
            //int maxWidth = Screen.width;
            _maxHeight = _rectTransform.rect.height;
            _maxWidth = _rectTransform.rect.width;
            float freeHeight = (_maxHeight - _data.Indent * 2) - (_data.NumberHeight * _rowsCount);
            float freeWidth = (_maxWidth - _data.Indent * 2) - (_data.NumberWidht * _columnsCount);
            _columnsIndent = freeWidth / (_columnsCount + 2 - 1);
            _rowsIndent = freeHeight / (_rowsCount + 2 - 1);
        }

        private void FindColumnsAndRowsCount()
        {
            int countAll = _selectNumbers.Length;
            int qnty = Convert.ToInt32(Math.Sqrt(countAll));
            int firstSize;
            int secondSize;
            for (int i = qnty; i > 0; i--)
            {
                if (countAll % i == 0 && countAll / i <= 10)
                {
                    firstSize = countAll / i;
                    secondSize = i;
                    if (firstSize > secondSize)
                    {
                        _rowsCount = secondSize;
                        _columnsCount = firstSize;
                    }
                    else
                    {
                        _rowsCount = firstSize;
                        _columnsCount = secondSize;
                    }
                    _lastRowColumnsCount = _columnsCount;
                    CheckRightCalcColumnsAndRows();
                    return;
                }
            }
            _columnsCount = qnty;
            _rowsCount = qnty;
            while (countAll - _columnsCount * (_rowsCount - 1) > _columnsCount)
            {
                _columnsCount++;
            }
            _lastRowColumnsCount = countAll - _columnsCount * (_rowsCount - 1);
            CheckRightCalcColumnsAndRows();
        }

        private void CheckRightCalcColumnsAndRows()
        {
            if ((_rowsCount - 1) * _columnsCount + _lastRowColumnsCount != _selectNumbers.Length)
            {
                throw new Exception($"Ошибка расчетов. Всего мест: {_selectNumbers.Length}. Columns = {_columnsCount}. Rows = {_rowsCount}. LastRowColumns = {_lastRowColumnsCount}");
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectNumberPanelPlayer))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineSelectNumbers()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        [ContextMenu(nameof(DefineSelectNumbers))]
        private ComponentAttachInfo DefineSelectNumbers()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _selectNumbers);
        }
        #endregion 
    }
}