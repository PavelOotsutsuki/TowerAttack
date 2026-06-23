using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameFields.Persons.Discovers;
using Tools.UI;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionPlayerDiscover : Discover
    {
        [SerializeField] private DiscoverLabel _discoverLabel;

        private CancellationTokenSource _currentCTS;

        public override void Init(CancellationToken gameFieldToken)
        {
            _discoverLabel.Init();

            base.Init(gameFieldToken);
        }

        public override void Activate(DiscoverActivateData data)
        {
            if (Token.IsCancellationRequested)
                return;

            base.Activate(data);

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(Token);

            LabelActivateDataAsync labelData = new LabelActivateDataAsync(new LabelActivateData(data.ActivateMessage), _currentCTS.Token);

            _discoverLabel.Show(labelData);
        }

        protected override void Deactivate()
        {
            if (Token.IsCancellationRequested)
                return;

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(Token);

            _discoverLabel.Hide();

            Deactivating(_currentCTS.Token).Forget();
        }

        private async UniTask Deactivating(CancellationToken token)
        {
            if (Token.IsCancellationRequested)
                return;

            try
            {
                await UniTask.WaitUntil(() => _discoverLabel.IsComplete, cancellationToken: token);

                base.Deactivate();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        //private void OnDisable()
        //{
        //    Utils.DestroyCTS(ref _currentCTS);
        //}

        #region AutomaticFillComponents

        [ContextMenu(nameof(DefineAllComponents) + nameof(StartTowerCardSelectionPlayerDiscover))]
        public override List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineDiscoverLabel()
            };

            list.AddRange(base.DefineAllComponents());

            return list;
        }

        [ContextMenu(nameof(DefineDiscoverLabel))]
        private ComponentAttachInfo DefineDiscoverLabel()
        {
           return AutomaticFillComponents.DefineComponent(this, ref _discoverLabel, ComponentLocationTypes.InChildren);
        }

        #endregion
    }
}