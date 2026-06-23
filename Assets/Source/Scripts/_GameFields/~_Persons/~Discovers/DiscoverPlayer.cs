using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.UI;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    [RequireComponent(typeof(CanvasGroup))]
    public class DiscoverPlayer : Discover
    {
        [SerializeField] private DiscoverPanel _discoverPanel;
        [SerializeField] private DiscoverLabel _discoverLabel;

        [SerializeField] private CanvasGroup _canvasGroup;

        private CancellationTokenSource _currentCTS;
        //private IHandBlockable _handBlockable;

        public override void Init(/*IHandBlockable handBlockable*/CancellationToken fightToken)
        {
            //_handBlockable = handBlockable;

            _canvasGroup.blocksRaycasts = true;

            _discoverPanel.Init();
            _discoverLabel.Init();

            base.Init(fightToken);
        }

        public override void Activate(DiscoverActivateData data)
        {
            if (Token.IsCancellationRequested)
                return;

            //_handBlockable.ForciblyBlock();
            _canvasGroup.blocksRaycasts = true;

            base.Activate(data);

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(Token);

            LabelActivateDataAsync labelData = new LabelActivateDataAsync(new LabelActivateData(data.ActivateMessage), _currentCTS.Token);

            _discoverPanel.Show(new CancellationTokenData(_currentCTS.Token));
            _discoverLabel.Show(labelData);
        }

        protected override void Deactivate()
        {
            if (Token.IsCancellationRequested)
                return;

            _canvasGroup.blocksRaycasts = false;

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(Token);

            _discoverPanel.Hide(new CancellationTokenData(_currentCTS.Token));
            _discoverLabel.Hide();

            Deactivating(_currentCTS.Token).Forget();
        }

        private async UniTask Deactivating(CancellationToken token)
        {
            if (Token.IsCancellationRequested)
                return;

            try
            {
                //TestCompletable(token).Forget();
                await UniTask.WaitUntil(() => _discoverLabel.IsComplete && _discoverPanel.IsComplete, cancellationToken: token);

                //_handBlockable.Unblock();
                base.Deactivate();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        //private async UniTask TestCompletable(CancellationToken token)
        //{
        //    try
        //    {
        //        while (true)
        //        {
        //            Debug.Log($"_discoverLabel.IsComplete: {_discoverLabel.IsComplete}");
        //            Debug.Log($"_discoverLabel.IsComplete: {_discoverPanel.IsComplete}");

        //            await UniTask.WaitForSeconds(1f, cancellationToken: token);
        //        }
        //    }
        //    catch (OperationCanceledException)
        //    {
        //        Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
        //    }

        //}

        //private void OnDisable()
        //{
        //    Utils.DestroyCTS(ref _currentCTS);
        //}

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(DiscoverPlayer))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineDiscoverPanel(),
                DefineDiscoverLabel(),
                DefineCanvasGroup()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineDiscoverPanel))]
        private ComponentAttachInfo DefineDiscoverPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _discoverPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDiscoverLabel))]
        private ComponentAttachInfo DefineDiscoverLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _discoverLabel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCanvasGroup))]
        private ComponentAttachInfo DefineCanvasGroup()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        }

        #endregion
    }
}