using Tools;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameFields.EndTurnButtons
{
    public class EndTurnButton : MonoBehaviour
    {
        [SerializeField] private Button _button;
        [SerializeField] private ChangeSideAnimator _changeSideAnimator;

        private IEndTurnHandler _endTurnHandler;

        public void Init(IEndTurnHandler endTurnHandler)
        {
            _endTurnHandler = endTurnHandler;

            _changeSideAnimator.Init(_button);
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
            _button.onClick.AddListener(OnClick);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void SetDeactiveSide()
        {
            _changeSideAnimator.PlayLockButtonAnimation();
            _endTurnHandler.OnEndTurn();
        }
    }
}
