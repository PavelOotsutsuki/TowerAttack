using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameFields.Persons.SelectMenues.Commons;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues.Attacks
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AttackMenu_OLD : MonoBehaviour, IAttackMenuActivator, IWorkable<AttackMenuActivateData>, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private AttackMenuLabel _attackMenuLabel;
        [SerializeField] private AttackMenuPanel _attackMenuPanel;

        [SerializeField] private CanvasGroup _canvasGroup;

        private ISelectResultHandler _attackResultHandler;
        private AttackResult _attackResult;
        private AttackMenuData _data;
        private AttackNumberPanel _attackNumberPanel;

        private IEnumerable<ICompletable> _completableElements;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete { get; private set; }

        private bool IsElementsComplete => _completableElements.Any(e => e.IsComplete == false) == false;
        //{
        //    get
        //    {
        //        foreach (ICompletable completableElement in _completableElements)
        //        {
        //            if (completableElement.IsComplete == false)
        //                return false;
        //        }

        //        return true;
        //    }
        //}

        public void Init(ISelectResultHandler attackResultHandler, AttackMenuData data, AttackNumberPanel attackNumberPanel)
        {
            gameObject.SetActive(false);
            IsComplete = false;
            //_canvasGroup.blocksRaycasts = false;
            _canvasGroup.blocksRaycasts = true;

            _data = data;
            _attackNumberPanel = attackNumberPanel;

            _attackResultHandler = attackResultHandler;
            _attackResult = null;

            _attackMenuLabel.Init();
            _attackMenuPanel.Init();
            //_attackNumberPanel.Init(cardNumberKeeper, _countNumbers);

            _completableElements = FillCompletableElements();
        }

        public virtual void Activate(AttackMenuActivateData activateData)
        {
            if (IsActive == true)
                return;

            IsComplete = false;
            IsActive = true;

            gameObject.SetActive(true);
            //_canvasGroup.blocksRaycasts = _data.IsInteractable;

            //LabelActivateData labelData = new LabelActivateData("Ожидаем противника...");
            //LabelActivateData labelData = new LabelActivateData("Выберете кого атакуем");
            LabelActivateData labelData = new LabelActivateData(_data.AttackMenuLabelText);
            _attackMenuLabel.Show(labelData);
            _attackMenuPanel.Show();

            _attackResult = new AttackResult();

            AttackNumberPanelActivateData numberPanelActivateData = new AttackNumberPanelActivateData(activateData.NeedSelect, _attackResult);
            _attackNumberPanel.Activate(numberPanelActivateData);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            //_canvasGroup.blocksRaycasts = false;

            Deactivating().ToUniTask();
        }

        protected virtual List<ICompletable> FillCompletableElements()
        {
            List<ICompletable> completables = new List<ICompletable>
            {
                _attackMenuLabel,
                _attackMenuPanel,
                _attackNumberPanel
            };

            return completables;
        }

        private IEnumerator Deactivating()
        {
            yield return OnDeactivating();

            _attackMenuLabel.Hide();
            _attackMenuPanel.Hide();

            yield return new WaitUntil(() => IsElementsComplete);

            gameObject.SetActive(false);

            if (_attackResult.IsAttackSuccess)
            {
                _attackResultHandler.SuccessChoice();
            }
            else
            {
                _attackResultHandler.FalledChoice();
            }

            IsComplete = true;
        }

        protected abstract IEnumerator OnDeactivating();

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackMenu_OLD))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineAttackMenuLabel(),
                DefineAttackMenuPanel(),
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

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}