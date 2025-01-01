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
    public class AttackMenu : MonoBehaviour, IWorkable, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private AttackMenuLabel _attackMenuLabel;
        [SerializeField] private AttackMenuPanel _attackMenuPanel;
        [SerializeField] private AttackButton _attackButton;
        [SerializeField] private AttackNumberPanel _attackNumberPanel;

        [SerializeField] private CanvasGroup _canvasGroup;

        //private IHandBlockable _handBlockable;
        private IAttackResultHandler _attackResultHandler;
        private AttackResult _attackResult;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete { get; private set; }

        //private ICardNumberKeeper _cardNumberKeeper;

        //public void Init(IHandBlockable handBlockable, ICardNumberKeeper cardNumberKeeper)
        //{
        //    _cardNumberKeeper = cardNumberKeeper;

        //    Init(handBlockable);
        //}

        public void Init(IAttackResultHandler attackResultHandler)
        {
            gameObject.SetActive(false);
            IsComplete = false;
            _canvasGroup.blocksRaycasts = false;

            //_handBlockable = handBlockable;
            _attackResultHandler = attackResultHandler;
            _attackResult = new AttackResult();

            _attackMenuLabel.Init();
            _attackMenuPanel.Init();
            _attackButton.Init(this);
            _attackNumberPanel.Init(_attackButton, attackResultHandler, _attackResult);
        }

        public void Activate()
        {
            if (IsActive == true)
                return;

            IsComplete = false;
            IsActive = true;

            //_handBlockable.ForciblyBlock();
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

            //_handBlockable.Unblock();
            gameObject.SetActive(false);

            if (_attackResult.IsAttackSuccess)
            {
                _attackResultHandler.SuccessAttack();
            }
            else
            {
                IsComplete = true;
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackMenu))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAttackMenuLabel(),
                DefineAttackMenuPanel(),
                DefineAttackButton(),
                DefineAttackNumberPanel(),
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineAttackMenuLabel))]
        private ComponentAttachInfo DefineAttackMenuLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _attackMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackMenuPanel))]
        private ComponentAttachInfo DefineAttackMenuPanel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _attackMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackButton))]
        private ComponentAttachInfo DefineAttackButton()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _attackButton, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineAttackNumberPanel))]
        private ComponentAttachInfo DefineAttackNumberPanel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _attackNumberPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        #endregion 
    }
}