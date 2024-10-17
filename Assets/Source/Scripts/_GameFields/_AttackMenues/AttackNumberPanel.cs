using System;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    public class AttackNumberPanel : MonoBehaviour
    {
        [SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private AttackNumber[] _attackNumbers;
        [SerializeField] private float _numberWidht = 100f;
        [SerializeField] private float _numberHeight = 100f;
        [SerializeField] private float _indent = 50f;

        private int _columnsCount;
        private int _rowsCount;
        private int _lastRowColumnsCount;

        private float _columnsIndent;
        private float _rowsIndent;

        private float _maxHeight;
        private float _maxWidth;

        public void Init(IActivatable attackButton)
        {
            FindColumnsAndRowsCount();
            FindIndents();
            InitNumbers(attackButton);

            _fadablePanel.Init();
        }

        public void Activate()
        {
            gameObject.SetActive(true);
            _fadablePanel.Show();
        }

        public void Deactivate()
        {
            _fadablePanel.Hide();
            //gameObject.SetActive(false);
        }

        //public void Unsubscribe()
        //{
        //    foreach (AttackNumber attackNumber in _attackNumbers)
        //    {
        //        attackNumber.Unsubscribe();
        //    }
        //}

        private void InitNumbers(IActivatable attackButton)
        {
            int number = 1;

            foreach (AttackNumber attackNumber in _attackNumbers)
            {
                attackNumber.Init(number, CalcNumberPosition(number), new Vector2(_numberWidht, _numberHeight), attackButton);
                number++;
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

            float x = _indent + _columnsIndent * column + _numberWidht / 2 + _numberWidht * (column - 1) - _maxWidth / 2;
            float y = _indent * (-1) + _maxHeight - (_rowsIndent * row + _numberHeight / 2 + _numberHeight * (row - 1)) - _maxHeight / 2;

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

            float freeHeight = (_maxHeight - _indent * 2) - (_numberHeight * _rowsCount);
            float freeWidth = (_maxWidth - _indent * 2) - (_numberWidht * _columnsCount);

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
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineFadablePanel();
            DefineRectTransform();
            DefineAttackNumbers();
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private void DefineFadablePanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineAttackNumbers))]
        private void DefineAttackNumbers()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackNumbers);
        }

        #endregion 
    }
}
