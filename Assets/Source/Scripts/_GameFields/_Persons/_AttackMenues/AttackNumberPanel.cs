using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Towers;
using Tools;
using Tools.UI;
using Tools.Utils.FillComponents;
using UnityEngine;
namespace GameFields.Persons.AttackMenues
{
    [RequireComponent(typeof(FadablePanel))]
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class AttackNumberPanel : MonoBehaviour, ICompletable, IWorkable<AttackNumberPanelActivateData>, IAutomaticFillComponents
    {
        [SerializeField] protected FadablePanel FadablePanel;
        [SerializeField] private CanvasGroup _canvasGroup;

        protected int NeedForActivate;

        protected ICardNumberKeeper CardNumberKeeper;
        protected AttackResult AttackResult;

        protected ConfirmableNumbers ConfirmableNumbers;
        protected int CountNumbers;

        protected bool IsCompleteThis;

        public bool IsComplete => IsCompleteThis && FadablePanel.IsComplete;

        public bool? IsActive { get; private set; } = null;

        public void Init(ICardNumberKeeper cardNumberKeeper, int countNumbers)
        {
            CardNumberKeeper = cardNumberKeeper;
            CountNumbers = countNumbers;
            AttackResult = null;

            ConfirmableNumbers = new ConfirmableNumbers();

            InitNumbers();

            FadablePanel.Init();
        }

        public void Activate(AttackNumberPanelActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            IsCompleteThis = false;
            _canvasGroup.blocksRaycasts = true;

            AttackResult = data.AttackResult;
            NeedForActivate = data.NeedForActivate;

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

        protected virtual void OnDeactivate()
        {
            IsCompleteThis = false;
        }

        protected abstract IEnumerator Deactivating();
        protected abstract void OnActivate();
        protected abstract void InitNumbers();

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(AttackNumberPanel))]
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