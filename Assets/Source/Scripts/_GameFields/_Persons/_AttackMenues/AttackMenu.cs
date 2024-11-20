using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Hands;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(CanvasGroup))]
    public class AttackMenu : MonoBehaviour, IWorkable, IAutomaticFillComponents
    {
        [SerializeField] private AttackMenuLabel _attackMenuLabel;
        [SerializeField] private AttackMenuPanel _attackMenuPanel;
        [SerializeField] private AttackButton _attackButton;
        [SerializeField] private AttackNumberPanel _attackNumberPanel;

        [SerializeField] private CanvasGroup _canvasGroup;

        private IHandBlockable _handBlockable;
        private ICardNumberKeeper _cardNumberKeeper;

        public bool? IsActive { get; private set; } = null;

        //private ICardNumberKeeper _cardNumberKeeper;

        //public void Init(IHandBlockable handBlockable, ICardNumberKeeper cardNumberKeeper)
        //{
        //    _cardNumberKeeper = cardNumberKeeper;

        //    Init(handBlockable);
        //}

        public void Init(IHandBlockable handBlockable, ICardNumberKeeper cardNumberKeeper)
        {
            gameObject.SetActive(false);
            _canvasGroup.blocksRaycasts = false;

            _handBlockable = handBlockable;
            _cardNumberKeeper = cardNumberKeeper;

            _attackMenuLabel.Init();
            _attackMenuPanel.Init();
            _attackButton.Init(Deactivate);
            _attackNumberPanel.Init(_attackButton);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _handBlockable.ForciblyBlock();
            gameObject.SetActive(true);
            _canvasGroup.blocksRaycasts = true;

            FadableLabelActivateData labelData = new FadableLabelActivateData("Выберете кого атакуем");
            _attackMenuLabel.Show(labelData);
            _attackMenuPanel.Show();
            //_attackButton.Activate();

            AttackNumberPanelActivateData numberPanelActivateData = new AttackNumberPanelActivateData(1);
            _attackNumberPanel.Activate(numberPanelActivateData);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _canvasGroup.blocksRaycasts = false;

            Deactivating().ToUniTask();
        }

        private IEnumerator Deactivating()
        {
            _attackButton.Deactivate();
            _attackNumberPanel.Deactivate();

            yield return new WaitUntil(() => _attackNumberPanel.IsComplete);

            _attackMenuLabel.Hide();
            _attackMenuPanel.Hide();

            yield return new WaitUntil(() => _attackMenuPanel.IsComplete && _attackMenuLabel.IsComplete && _attackButton.IsComplete && _attackNumberPanel.IsComplete);

            _handBlockable.Unblock();
            gameObject.SetActive(false);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackMenu))]
        public void DefineAllComponents()
        {
            DefineAttackMenuLabel();
            DefineAttackMenuPanel();
            DefineAttackButton();
            DefineAttackNumberPanel();
            DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineAttackMenuLabel))]
        private void DefineAttackMenuLabel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackMenuPanel))]
        private void DefineAttackMenuPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackButton))]
        private void DefineAttackButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackNumberPanel))]
        private void DefineAttackNumberPanel()
        {
            AutomaticFillComponents.DefineComponent(this, ref _attackNumberPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private void DefineCanvasGroup()
        {
            AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        #endregion 
    }
}