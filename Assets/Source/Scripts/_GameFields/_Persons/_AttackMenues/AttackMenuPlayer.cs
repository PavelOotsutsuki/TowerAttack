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
    public class AttackMenuPlayer : MonoBehaviour, IAttackMenu, IWorkable, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private AttackMenuLabel _attackMenuLabel;
        [SerializeField] private AttackMenuPanel _attackMenuPanel;
        [SerializeField] private AttackButton _attackButton;
        [SerializeField] private AttackNumberPanelPlayer _attackNumberPanel;

        [SerializeField] private CanvasGroup _canvasGroup;

        private IAttackResultHandler _attackResultHandler;
        private AttackResult _attackResult;

        private int _countNumbers;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete { get; private set; }

        public void Init(ICardNumberKeeper cardNumberKeeper, IAttackResultHandler attackResultHandler, int countNumbers)
        {
            gameObject.SetActive(false);
            IsComplete = false;
            _canvasGroup.blocksRaycasts = false;

            _countNumbers = countNumbers;
            _attackResultHandler = attackResultHandler;
            _attackResult = null;

            _attackMenuLabel.Init();
            _attackMenuPanel.Init();
            _attackButton.Init(this);
            _attackNumberPanel.Init(_attackButton, cardNumberKeeper, _countNumbers);
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

            _attackResult = new AttackResult();

            AttackNumberPanelActivateData numberPanelActivateData = new AttackNumberPanelActivateData(1, _attackResult);
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

            gameObject.SetActive(false);

            if (_attackResult.IsAttackSuccess)
            {
                _attackResultHandler.SuccessAttack();
                IsComplete = true;
            }
            else
            {
                _attackResultHandler.FalledAttack();
                IsComplete = true;
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(IAttackMenu))]
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