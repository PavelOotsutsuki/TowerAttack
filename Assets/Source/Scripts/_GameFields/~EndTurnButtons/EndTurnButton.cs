using System.Collections.Generic;
using System.Threading;
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

        public void Init(CancellationToken fightToken)
        {
            _changeSideAnimator = new ChangeSideAnimator(_data, _button , fightToken);

            //Deactivate();
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
            if (IsActive == true)
                return;

            IsActive = true;

            _changeSideAnimator.PlayUnlockButtonAnimation();

            //StartCoroutine(WaitingTest());
        }

        //private IEnumerator WaitingTest()
        //{
        //    yield return new WaitForSeconds(15f);

        //    Deactivate();
        //}

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

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
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineButton()
            };

            return list;
        }

        [ContextMenu(nameof(DefineButton))]
        private ComponentAttachInfo DefineButton()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _button, ComponentLocationTypes.InThisElseChildren);
        }

        //[ContextMenu(nameof(DefineCanvasGroup))]
        //private void DefineCanvasGroup()
        //{
        //    AutomaticFillComponents.DefineComponent(this, ref _canvasGroup, ComponentLocationTypes.InThis);
        //}
        #endregion
    }
}