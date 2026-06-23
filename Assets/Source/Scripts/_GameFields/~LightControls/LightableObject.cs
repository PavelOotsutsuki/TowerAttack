using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Tools;
using Tools.Utils;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.LightControls
{
    public abstract class LightableObject : MonoBehaviour, IViewable, IAutomaticFillComponents
    {
        [SerializeField] private Transform _transform;
        [SerializeField] private LightFrame _lightFrame;
        [SerializeField] private LightObjectsParent _lightObjectsParent;

        private Transform _defaultParent;
        private Transform _lightParent;
        private CancellationToken _fightToken;

        private CancellationTokenSource _currentCTS;
        private Coroutine _currentCoroutine;

        public bool? IsShown { get; private set; } = null;

        public void Init(CancellationToken fightToken)
        {
            _fightToken = fightToken;

            _defaultParent = _transform.parent;
            _lightParent = _lightObjectsParent.GetTransform();

            _currentCoroutine = null;

            _lightFrame.Init();
        }

        public void Show()
        {
            if (IsShown == true)
                return;

            IsShown = true;

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            _lightFrame.Show(new CancellationTokenData(_currentCTS.Token));
            _transform.SetParent(_lightParent);
        }

        public void Hide()
        {
            if (IsShown == false)
                return;

            IsShown = false;

            Utils.DestroyCTS(ref _currentCTS);
            _currentCTS = CancellationTokenSource.CreateLinkedTokenSource(_fightToken);

            _lightFrame.Hide(new CancellationTokenData(_currentCTS.Token));

            Hiding(_currentCTS.Token).Forget();
        }

        private async UniTask Hiding(CancellationToken token)
        {
            await UniTask.WaitUntil(() => _lightFrame.IsComplete, cancellationToken: token);

            //if (IsShown == false)
                _transform.SetParent(_defaultParent);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LightableObject))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineTransform(),
                DefineLightFrame(),
                DefineLightObjectsParent()
            };

            return list;
        }

        [ContextMenu(nameof(DefineTransform))]
        private ComponentAttachInfo DefineTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _transform, ComponentLocationTypes.InThis);
        }

        [ContextMenu(nameof(DefineLightFrame))]
        private ComponentAttachInfo DefineLightFrame()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lightFrame, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineLightObjectsParent))]
        private ComponentAttachInfo DefineLightObjectsParent()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lightObjectsParent, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}