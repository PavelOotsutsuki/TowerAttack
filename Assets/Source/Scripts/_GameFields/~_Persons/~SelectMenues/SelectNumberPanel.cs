using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameFields.Persons;
using GameFields.Persons.ConfirmableNumbersView;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    [RequireComponent(typeof(FadablePanel))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class SelectNumberPanel : MonoBehaviour, ICompletable, IWorkable<SelectNumberPanelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] protected FadablePanel FadablePanel;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected int NeedForActivate;

        protected ICardNumberKeeper CardNumberKeeper;
        protected SelectResult SelectResult;

        protected SelectNumbersList SelectedNumbers;
        protected ConfirmableNumbers ConfirmableNumbers;
        protected int[] CardNumbers;
        protected List<ISelectNumber> CurrentAvailableNumbers;
        protected LastSelectedNumbersWatcher LastSelectedNumbersWatcher;

        protected bool IsConsecutiveMode;

        protected bool IsCompleteThis;

        private ISelectNumber[] _selectNumbers;

        public bool IsComplete => IsCompleteThis && FadablePanel.IsComplete;

        public bool? IsActive { get; private set; } = null;

        protected void Init(ICardNumberKeeper cardNumberKeeper, int[] сardNumbers, SelectNumbersList selectedNumbers,
            ConfirmableNumbers confirmableNumbers, ISelectNumber[] selectNumbers,
            LastSelectedNumbersWatcher lastSelectedNumbersWatcher)
        {
            CardNumberKeeper = cardNumberKeeper;
            CardNumbers = сardNumbers;
            ConfirmableNumbers = confirmableNumbers;
            LastSelectedNumbersWatcher = lastSelectedNumbersWatcher;

            ClearCurrentVariables();
            //SelectResult = null;

            SelectedNumbers = selectedNumbers;

            _selectNumbers = selectNumbers;

            InitNumbers();

            FadablePanel.Init();
        }

        public void Activate(SelectNumberPanelActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            IsCompleteThis = false;
            _canvasGroup.blocksRaycasts = true;

            SelectResult = data.SelectResult;
            NeedForActivate = data.NeedForActivate;

            SetRestriction(data.RestrictionType);

            gameObject.SetActive(true);

            FadablePanel.Show();

            OnActivate();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            _canvasGroup.blocksRaycasts = false;

            OnDeactivate();

            Deactivating().ToUniTask();
        }

        public void OnDisable()
        {
            ClearCurrentVariables();
        }

        protected virtual void OnDeactivate()
        {
            IsCompleteThis = false;
        }

        protected abstract IEnumerator Deactivating();
        protected abstract void OnActivate();
        protected abstract void InitNumbers();

        private void ClearCurrentVariables()
        {
            _canvasGroup.blocksRaycasts = false;
            CurrentAvailableNumbers = null;
            IsConsecutiveMode = false;
            NeedForActivate = -1;
            SelectResult = null;
        }

        private void SetRestriction(RestrictionType? restrictionType)
        {
            CurrentAvailableNumbers = new List<ISelectNumber>();

            if (restrictionType == null)
            {
                foreach (ISelectNumber selectNumber in _selectNumbers)
                {
                    CurrentAvailableNumbers.Add(selectNumber);
                }

                return;
            }

            switch (restrictionType.Value)
            {
                case RestrictionType.Even:
                    SetEvenNumbers();
                    break;
                case RestrictionType.Odd:
                    SetOddNumbers();
                    break;
                case RestrictionType.Consecutive:
                    SetConsecutiveNumbers();
                    break;
                default:
                    throw new ArgumentNullException($"Неизвестный {nameof(RestrictionType)}: {restrictionType}");
            }
        }

        private void SetEvenNumbers()
        {
            foreach (ISelectNumber selectNumber in _selectNumbers)
            {
                if (selectNumber.Number % 2 == 0)
                    CurrentAvailableNumbers.Add(selectNumber);
            }
        }

        private void SetOddNumbers()
        {
            foreach (ISelectNumber selectNumber in _selectNumbers)
            {
                if (selectNumber.Number % 2 == 1)
                    CurrentAvailableNumbers.Add(selectNumber);
            }
        }

        private void SetConsecutiveNumbers()
        {
            IsConsecutiveMode = true;

            foreach (ISelectNumber selectNumber in _selectNumbers)
            {
                CurrentAvailableNumbers.Add(selectNumber);
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectNumberPanel))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineFadablePanel(),
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineFadablePanel))]
        private ComponentAttachInfo DefineFadablePanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref FadablePanel, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        #endregion 
    }
}