using Cards;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.EndTurnButtons
{
    public class EndTurnButton : MonoBehaviour
    {
        [SerializeField] private ActiveView _activeView;
        [SerializeField] private DeactiveView _deactiveView;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private float _activeViewInvertDuration = 0.2f;
        [SerializeField] private float _deactiveViewInvertDuration = 0.2f;

        private IEndTurnHandler _drawHandler;
        private ChangeSideAnimator _changeSideAnimator;

        public void Init(IEndTurnHandler drawHandler)
        {
            _drawHandler = drawHandler;
            _changeSideAnimator = new ChangeSideAnimator(_activeView, _deactiveView, _rectTransform);
        }

        private void OnEnable()
        {
            Debug.Log("OnEnable");
            _activeView.AddOnClickListener(OnClick);
        }

        private void OnDisable()
        {
            Debug.Log("OnDisable");
            _activeView.RemoveOnClickListener(OnClick);
        }

        public void OnClick()
        {
            Debug.Log("OnClick");
            SetDeactiveSide();
            _drawHandler.OnEndTurn();
        }

        private void SetActiveSide()
        {

        }

        private void SetDeactiveSide()
        {
            //StartCoroutine(_changeSideAnimator.PlayLockButtonAnimation());
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
