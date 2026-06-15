using System.Collections.Generic;
using System.Threading;
using DG.Tweening;
using Tools.Utils.FillComponents;
using UnityEngine;
using Cysharp.Threading.Tasks;
using Tools.Extensions;

namespace Tools.UI
{
    public class NascentPanel : MonoBehaviour, ICompletable, IWorkable<CancellationTokenData>, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;

        [SerializeField] private NascentData _data;

        private CancellationTokenSource _currentCTS;

        public bool IsComplete { get; private set; }
        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            IsComplete = true;

            Deactivate();
        }

        public void Activate(CancellationTokenData tokenData)
        {
            if (IsActive == true)
                return;

            IsActive = true;
            IsComplete = false;

            Utils.Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(tokenData.Token);

            _transform.DOScale(_data.EndScale, _data.Duration).OnComplete(() => IsComplete = true).ToUniTask(ct: _currentCTS.Token).Forget();
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;
            Utils.Utils.DestroyCTS(ref _currentCTS);

            _transform.localScale = _data.StartScale;
        }

        private void OnDisable()
        {
            Utils.Utils.DestroyCTS(ref _currentCTS);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(NascentPanel))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }
        #endregion
    }
}