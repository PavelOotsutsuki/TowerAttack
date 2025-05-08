using System;
using System.Collections;
using System.Collections.Generic;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public class AttackNumberPanelPlayer : AttackNumberPanel
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private AttackNumber[] _attackNumbers;
        [SerializeField] private AttackNumberPanelPlayerData _data;

        private int _columnsCount;
        private int _rowsCount;
        private int _lastRowColumnsCount;
        private float _columnsIndent;
        private float _rowsIndent;
        private float _maxHeight;
        private float _maxWidth;

        private int _activateCounter;

        private IWorkable _attackButton;

        private bool _isCompleteNumbersHide;

        public bool IsCompleteNumbersHide => _isCompleteNumbersHide;

        public void Init(IWorkable attackButton, ICardNumberKeeper cardNumberKeeper, int countNumbers,
            ConfirmableNumbers confirmableNumbers)
        {
            if (_attackNumbers.Length != countNumbers)
                throw new Exception("Несовпадение заданного кол-ва номеров и кол-ва объектов AttackNumber");

            _attackButton = attackButton;

            base.Init(cardNumberKeeper, countNumbers, confirmableNumbers);
        }

        protected override void InitNumbers()
        {
            _activateCounter = 0;

            FindColumnsAndRowsCount();
            FindIndents();

            int number = 1;

            foreach (AttackNumber attackNumber in _attackNumbers)
            {
                attackNumber.Init(number, CalcNumberPosition(number), new Vector2(_data.NumberWidht, _data.NumberHeight), OnAttackNumberClick);
                number++;
            }
        }

        protected override void OnActivate()
        {
            _activateCounter = 0;

            foreach (AttackNumber attackNumber in _attackNumbers)
            {
                attackNumber.Activate();
            }

            IsCompleteThis = true;
        }

        protected override void OnDeactivate()
        {
            _isCompleteNumbersHide = false;

            base.OnDeactivate();
        }

        protected override IEnumerator Deactivating()
        {
            List<AttackNumber> selectedNumbers = new List<AttackNumber>(); // Можно заменить на LINQ

            foreach (AttackNumber attackNumber in _attackNumbers)
            {
                if (attackNumber.IsClicked)
                {
                    selectedNumbers.Add(attackNumber);
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

            foreach (AttackNumber selectedNumber in selectedNumbers)
            {
                if (CardNumberKeeper.Card.IsSuccessAttack(selectedNumber.Number))
                {
                    selectedNumber.SuccessChoice();
                    AttackResult.SuccessChoice();
                }
                else
                {
                    selectedNumber.ErrorChoice();
                    ConfirmableNumbers.AddAccept(selectedNumber);
                }

                float delayUntilPlayNextSelectedNumberAnimation = selectedNumber.AnimationDuration * _data.NextAnimationStartPercent;

                yield return new WaitForSeconds(delayUntilPlayNextSelectedNumberAnimation);
            }

            float waitLastAnimationCompleted = selectedNumbers[selectedNumbers.Count - 1].AnimationDuration * (1f - _data.NextAnimationStartPercent);
            yield return new WaitForSeconds(waitLastAnimationCompleted + _data.DelayAfterAllNumbersAnimationsPlayed);

            _isCompleteNumbersHide = true;

            FadablePanel.Hide();

            foreach (AttackNumber attackNumber in _attackNumbers)
            {
                attackNumber.Deactivate();
            }

            yield return new WaitUntil(() => FadablePanel.IsComplete);

            IsCompleteThis = true;
        }

        private void OnAttackNumberClick(bool isActive)
        {
            _activateCounter += isActive ? 1 : -1;

            if (_activateCounter == NeedForActivate)
            {
                _attackButton.Activate();
            }
            else
            {
                _attackButton.Deactivate();
            }
        }

        private Vector2 CalcNumberPosition(int number)
        {
            if (number < 1 && number > _attackNumbers.Length)
            {
                throw new ArgumentOutOfRangeException($"Такого number-a нет! Number: {number}. MaxLenght: {_attackNumbers.Length}");
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
            int countAll = _attackNumbers.Length;
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
            if ((_rowsCount - 1) * _columnsCount + _lastRowColumnsCount != _attackNumbers.Length)
            {
                throw new Exception($"Ошибка расчетов. Всего мест: {_attackNumbers.Length}. Columns = {_columnsCount}. Rows = {_rowsCount}. LastRowColumns = {_lastRowColumnsCount}");
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumberPanelPlayer))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform(),
                DefineAttackNumbers()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        [ContextMenu(nameof(DefineAttackNumbers))]
        private ComponentAttachInfo DefineAttackNumbers()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _attackNumbers);
        }
        #endregion 
    }
}