using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    public class TowerCardView : MonoBehaviour, IWorkable<TowerBigCardShowData>, IAutomaticFillComponents
    {
        [SerializeField] private TowerCardViewPanel _viewPanel;
        [SerializeField] private TowerBigCard _bigCard;

        //private Coroutine _deactivateCoroutine = null; 
        //private Coroutine _activateCoroutine = null;
        private CancellationTokenSource _activateCTS;
        private CancellationTokenSource _deactivateCTS;

        private CancellationToken _fightToken;

        public bool? IsActive { get; private set; } = null;

        public void Init(CancellationToken fightToken)
        {
            _fightToken = fightToken;

            _viewPanel.Init();
            _bigCard.Init(fightToken);

            gameObject.SetActive(false);
        }

        public void Activate(TowerBigCardShowData data)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            if (IsActive == true)
                return;

            IsActive = true;

            gameObject.SetActive(true);

            Utils.DestroyCTS(ref _activateCTS);
            _activateCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            Activating(data, _activateCTS.Token).Forget();
        }

        public void Deactivate()
        {
            if (_fightToken.IsCancellationRequested)
                return;

            if (IsActive == false)
                return;

            IsActive = false;

            Utils.DestroyCTS(ref _activateCTS);
            Utils.DestroyCTS(ref _deactivateCTS);
            _deactivateCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            _viewPanel.Hide(new CancellationTokenData(_deactivateCTS.Token));
            _bigCard.Hide();

            WaitUntilSetDeactivate(_deactivateCTS.Token).Forget();
        }

        //private void OnDisable()
        //{
        //    Utils.DestroyCTS(ref _activateCTS);
        //    Utils.DestroyCTS(ref _deactivateCTS);
        //}

        private async UniTask Activating(TowerBigCardShowData data, CancellationToken activateToken)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            try
            {
                if (_deactivateCTS != null)
                {
                    Utils.DestroyCTS(ref _deactivateCTS);
                }
                else
                {
                    await UniTask.WaitForSeconds(0.8f, cancellationToken: activateToken);
                }

                _viewPanel.Show(new CancellationTokenData(activateToken));
                _bigCard.Show(data);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        private async UniTask WaitUntilSetDeactivate(CancellationToken token)
        {
            if (_fightToken.IsCancellationRequested)
                return;

            try
            {
                await UniTask.WaitUntil(() => _viewPanel.IsComplete && _bigCard.IsComplete, cancellationToken: token);

                gameObject.SetActive(false);
                Utils.DestroyCTS(ref _deactivateCTS);
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"ОТМЕНА ТОКЕНА: {MethodBase.GetCurrentMethod().DeclaringType.Name}: {GetType().Name}");
            }
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(TowerCardView))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTowerCardViewPanel(),
                DefineTowerBigCard()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTowerCardViewPanel))]
        private ComponentAttachInfo DefineTowerCardViewPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _viewPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineTowerBigCard))]
        private ComponentAttachInfo DefineTowerBigCard()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _bigCard, ComponentLocationTypes.InChildren);
        }
        #endregion
    }
}