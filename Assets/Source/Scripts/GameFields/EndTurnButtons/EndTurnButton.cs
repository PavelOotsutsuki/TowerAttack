using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace GameFields.EndTurnButtons
{
    public class EndTurnButton : MonoBehaviour
    {
        [SerializeField] private ActiveView _activeView;
        [SerializeField] private DeactiveView _deactiveView;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _activeViewInvertDuration = 0.2f;
        [SerializeField] private float _deactiveViewInvertDuration = 0.2f;

        private ChangeSideAnimator _changeSideAnimator;

        public void Init(IEndTurnHandler drawHandler)
        {
            _changeSideAnimator = new ChangeSideAnimator(_activeView.gameObject, _deactiveView.gameObject, _rectTransform, _activeViewInvertDuration, _deactiveViewInvertDuration, drawHandler);
        }

        public void SetActiveSide()
        {
            _changeSideAnimator.PlayUnlockButtonAnimation();
        }

        public void OnClick()
        {
            SetDeactiveSide();
        }

        private void OnEnable()
        {
            _activeView.AddOnClickListener(OnClick);
        }

        private void OnDisable()
        {
            _activeView.RemoveOnClickListener(OnClick);
        }

        private void SetDeactiveSide()
        {
            _changeSideAnimator.PlayLockButtonAnimation();
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineActiveView();
            DefineDeactiveView();
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineActiveView))]
        private void DefineActiveView()
        {
            AutomaticFillComponents.DefineComponent(this, ref _activeView, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineDeactiveView))]
        private void DefineDeactiveView()
        {
            AutomaticFillComponents.DefineComponent(this, ref _deactiveView, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
    }
}
