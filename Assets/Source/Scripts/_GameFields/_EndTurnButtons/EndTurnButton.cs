using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.EndTurnButtons
{
    public class EndTurnButton : MonoBehaviour, IWorkable, IEndTurnButtonStateWatcher, IAutomaticFillComponents
    {
        [SerializeField] private ChangeSideAnimatorData _data;
        [SerializeField] private Button _button;
        //[SerializeField] private CanvasGroup _canvasGroup;

        private ChangeSideAnimator _changeSideAnimator;

        public bool EndTurnClicked => _changeSideAnimator.IsActiveSide;
        public bool? IsActive { get; private set; } = null;

        public void Init()
        {
            _changeSideAnimator = new ChangeSideAnimator(_data, _button);
            _changeSideAnimator.PlayLockButtonAnimation();
        }

        //public void Activate()
        //{
        //    if (IsActive == true)
        //        return;

        //    IsActive = true;

        //    _button.interactable = true;
        //    Debug.Log("Activate: " + _button.interactable);
        //}

        //public void Deactivate()
        //{
        //    if (IsActive == false)
        //        return;

        //    IsActive = false;

        //    _button.interactable = false;
        //    Debug.Log("Deactivate: " + _button.interactable);
        //}

        public void Activate()
        {
            _changeSideAnimator.PlayUnlockButtonAnimation();
        }

        public void Deactivate()
        {
            _changeSideAnimator.PlayLockButtonAnimation();
        }

        private void OnClick()
        {
            Deactivate();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(EndTurnButton))]
        public void DefineAllComponents()
        {
            DefineButton();
            //DefineCanvasGroup();
        }

        [ContextMenu(nameof(DefineButton))]
        private void DefineButton()
        {
            AutomaticFillComponents.DefineComponent(this, ref _button, ComponentLocationTypes.InThisElseChildren);
        }

        //[ContextMenu(nameof(DefineCanvasGroup))]
        //private void DefineCanvasGroup()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        //}
        #endregion
    }
}