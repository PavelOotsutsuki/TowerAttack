using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(FadablePanel))]
    public class AttackNumberPanelPlayer : MonoBehaviour, ICompletable, IWorkable<AttackNumberPanelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] private FadablePanel _fadablePanel;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private AttackNumber[] _attackNumbers;
        [SerializeField] private float _numberWidht = 100f;
        [SerializeField] private float _numberHeight = 100f;
        [SerializeField] private float _indent = 50f;

        //[SerializeField] private Sprite _disableSprite;

        private int _columnsCount;
        private int _rowsCount;
        private int _lastRowColumnsCount;

        private float _columnsIndent;
        private float _rowsIndent;

        private float _maxHeight;
        private float _maxWidth;

        private int _activateCounter;
        private int _needForActivate;

        private IWorkable _attackButton;
        private ICardNumberKeeper _cardNumberKeeper;
        private AttackResult _attackResult;

        private ConfirmableNumbers _confirmableNumbers;

        private bool _isComplete;

        public bool IsComplete => _isComplete && _fadablePanel.IsComplete;
        public bool? IsActive { get; private set; } = null;

        public void Init(IWorkable attackButton, ICardNumberKeeper cardNumberKeeper, int countNumbers)
        {
            if (_attackNumbers.Length != countNumbers)
                throw new Exception("Несовпадение заданного кол-ва номеров и кол-ва объектов AttackNumber");

            _attackButton = attackButton;
            _cardNumberKeeper = cardNumberKeeper;
            _attackResult = null;

            _confirmableNumbers = new ConfirmableNumbers();

            _activateCounter = 0;

            FindColumnsAndRowsCount();
            FindIndents();
            InitNumbers();

            _fadablePanel.Init();
        }

        public void Activate(AttackNumberPanelActivateData data)
        {
            if (IsActive == true)
                return;

            _isComplete = false;

            _needForActivate = data.NeedForActivate;
            _attackResult = data.AttackResult;
            _activateCounter = 0;

            gameObject.SetActive(true);

            foreach (AttackNumber attackNumber in _attackNumbers)
            {
                //if (data.DisableNumbers.Contains(attackNumber))
                //{
                //    attackNumber.Activate(new AttackNumberActivateData(true));
                //}
                //else
                //{
                //    attackNumber.Activate(new AttackNumberActivateData(false));
                //}
                attackNumber.Activate();
            }

            _fadablePanel.Show();

            IsActive = true;
            _isComplete = true;
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            _isComplete = false;

            Deactivating().ToUniTask();

            IsActive = false;
            //gameObject.SetActive(false);
        }

        private IEnumerator Deactivating()
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
                if (_cardNumberKeeper.Card.IsSuccessAttack(selectedNumber.Number))
                {
                    selectedNumber.SuccessChoice();
                    _attackResult.SuccessChoice();
                }
                else
                {
                    selectedNumber.ErrorChoice();
                    _confirmableNumbers.Add(selectedNumber);
                }

                yield return new WaitForSeconds(0.8f);
            }

            yield return new WaitForSeconds(1f);

            _fadablePanel.Hide();

            foreach (AttackNumber attackNumber in _attackNumbers)
            {
                attackNumber.Deactivate();
            }

            _isComplete = true;
        }

        //public void Unsubscribe()
        //{
        //    foreach (AttackNumber attackNumber in _attackNumbers)
        //    {
        //        attackNumber.Unsubscribe();
        //    }
        //}
        private void OnAttackNumberClick(bool isActive)
        {
            _activateCounter += isActive ? 1 : -1;

            if (_activateCounter == _needForActivate)
            {
                _attackButton.Activate();
            }
            else
            {
                _attackButton.Deactivate();
            }
        }

        private void InitNumbers()
        {
            int number = 1;

            foreach (AttackNumber attackNumber in _attackNumbers)
            {
                attackNumber.Init(number, CalcNumberPosition(number), new Vector2(_numberWidht, _numberHeight), OnAttackNumberClick);
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
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumberPanel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadablePanel(),
                DefineRectTransform(),
                DefineAttackNumbers()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _fadablePanel, ComponentLocationTypes.InThis);
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