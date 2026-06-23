using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.UI;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.SelectMenues
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class SelectMenu : MonoBehaviour, ISelectMenuActivator, IWorkable<SelectMenuActivateData>, ICompletable, IAutomaticFillComponents
    {
        [SerializeField] private SelectMenuLabel _selectMenuLabel;
        [SerializeField] private SelectMenuPanel _selectMenuPanel;

        [SerializeField] private CanvasGroup _canvasGroup;

        //protected ISelectResultHandler SelectResultHandler;
        private ISelectResultHandler _selectResultHandler;

        private SelectResult _selectResult;
        private SelectNumberPanel _selectNumberPanel;
        private SelectMenuLabelTextLogic _selectMenuLabelTextLogic;

        protected CancellationToken FightToken;
        protected CancellationTokenSource CurrentCTS;

        private IEnumerable<ICompletable> _completableElements;

        public bool? IsActive { get; private set; } = null;
        public bool IsComplete { get; private set; }

        private bool IsElementsComplete => _completableElements.Any(e => e.IsComplete == false) == false;

        public void Init(ISelectResultHandler selectResultHandler, SelectNumberPanel selectNumberPanel,
            SelectMenuLabelTextLogic selectMenuLabelTextLogic, CancellationToken fightToken)
        {
            gameObject.SetActive(false);
            IsComplete = false;

            _canvasGroup.blocksRaycasts = true;

            _selectNumberPanel = selectNumberPanel;
            FightToken = fightToken;

            //SelectResultHandler = selectResultHandler;
            _selectResultHandler = selectResultHandler;
            _selectResult = null;

            _selectMenuLabel.Init();
            _selectMenuPanel.Init();

            _selectMenuLabelTextLogic = selectMenuLabelTextLogic;
            _completableElements = FillCompletableElements();
        }

        public virtual void Activate(SelectMenuActivateData activateData)
        {
            if (FightToken.IsCancellationRequested)
                return;

            if (IsActive == true)
                return;

            IsComplete = false;
            IsActive = true;

            gameObject.SetActive(true);

            Utils.DestroyCTS(ref CurrentCTS);
            CurrentCTS = CancellationTokenSource.CreateLinkedTokenSource(FightToken);

            LabelActivateDataAsync labelData = new LabelActivateDataAsync(new LabelActivateData(_selectMenuLabelTextLogic.CreateLabelText()), CurrentCTS.Token);
            _selectMenuLabel.Show(labelData);
            _selectMenuPanel.Show(new CancellationTokenData(CurrentCTS.Token));

            _selectResult = new SelectResult();

            SelectNumberPanelActivateData numberPanelActivateData = new SelectNumberPanelActivateData(activateData.NeedSelect, activateData.RestrictionType, _selectResult);
            _selectNumberPanel.Activate(numberPanelActivateData);
        }

        public void Deactivate()
        {
            if (FightToken.IsCancellationRequested)
                return;

            if (IsActive == false)
                return;

            IsActive = false;

            //Utils.DestroyCTS(ref CurrentCTS);
            //CurrentCTS = CancellationTokenSource.CreateLinkedTokenSource(FightToken);

            Deactivating(CurrentCTS.Token).Forget();
        }

        protected virtual List<ICompletable> FillCompletableElements()
        {
            List<ICompletable> completables = new List<ICompletable>
            {
                _selectMenuLabel,
                _selectMenuPanel,
                _selectNumberPanel
            };

            return completables;
        }

        private async UniTask Deactivating(CancellationToken token)
        {
            if (FightToken.IsCancellationRequested)
                return;

            try
            {
                await OnDeactivating(token);

                _selectMenuLabel.Hide(new CancellationTokenData(token));
                _selectMenuPanel.Hide(new CancellationTokenData(token));

                await UniTask.WaitUntil(() => IsElementsComplete, cancellationToken: token);

                gameObject.SetActive(false);

                //SetSelectResultData setSelectResultData;

                //if (_selectResult.IsSelectSuccess)
                //{
                //    setSelectResultData = new SetSelectResultData(ResultType.Success);
                //    //_selectResultHandler.SuccessChoice();
                //}
                //else
                //{
                //    setSelectResultData = new SetSelectResultData(ResultType.Falled);
                //    //_selectResultHandler.FalledChoice();
                //}

                //SelectResultHandler.SetResult(_selectResult.Data);
                _selectResultHandler.SetResult(_selectResult.Data);

                await UniTask.WaitUntil(() => _selectResultHandler.IsComplete, cancellationToken: token);

                IsComplete = true;
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        //private void OnDisable()
        //{
        //    Utils.DestroyCTS(ref CurrentCTS);
        //}

        protected abstract UniTask OnDeactivating(CancellationToken token);

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(SelectMenu))]
        public virtual List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineSelectMenuLabel(),
                DefineSelectMenuPanel(),
                DefineCanvasGroup()
            };

            return list;
        }

        [ContextMenu(nameof(DefineSelectMenuLabel))]
        private ComponentAttachInfo DefineSelectMenuLabel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _selectMenuLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineSelectMenuPanel))]
        private ComponentAttachInfo DefineSelectMenuPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _selectMenuPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}