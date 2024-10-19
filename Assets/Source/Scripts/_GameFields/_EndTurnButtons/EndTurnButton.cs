using UnityEngine;
using UnityEngine.UI;

namespace GameFields.EndTurnButtons
{
    public class EndTurnButton : MonoBehaviour, IButtonActivator
    {
        [SerializeField] private Button _button;
        [SerializeField] private ChangeSideAnimator _changeSideAnimator;

        public bool IsActive => _changeSideAnimator.IsActiveSide;

        public void Init()
        {
            _changeSideAnimator.Init(_button);
            _changeSideAnimator.PlayLockButtonAnimation();
        }

        public void SetActiveSide()
        {
            _changeSideAnimator.PlayUnlockButtonAnimation();
        }

        private void OnClick()
        {
            SetDeactiveSide();
        }

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void SetDeactiveSide()
        {
            _changeSideAnimator.PlayLockButtonAnimation();
        }
    }
}
