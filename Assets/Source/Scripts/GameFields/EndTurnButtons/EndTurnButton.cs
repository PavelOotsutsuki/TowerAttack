using Cards;
using Tools;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameFields.EndTurnButtons
{
    public class EndTurnButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ActiveView _activeView;
        [SerializeField] private DeactiveView _deactiveView;
        [SerializeField] private float _activeViewInvertDuration = 0.2f;
        [SerializeField] private float _deactiveViewInvertDuration = 0.2f;

        private IEndTurnHandler _drawHandler;
        private ChangeSideAnimator _changeSideAnimator;

        public void Init(IEndTurnHandler drawHandler)
        {
            _drawHandler = drawHandler;
            _changeSideAnimator = new ChangeSideAnimator(_activeView, _deactiveView);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            SetDeactiveSide();
            _drawHandler.OnEndTurn();
        }

        private void SetActiveSide()
        {

        }

        private void SetDeactiveSide()
        {
            StartCoroutine(_changeSideAnimator.LockButton());
        }

        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineActiveView();
            DefineDeactiveView();
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
    }
}
