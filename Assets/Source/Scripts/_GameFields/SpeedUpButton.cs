using Tools.UI.Buttons;
using UnityEngine;

namespace GameFields
{
    public class SpeedUpButton : SelectableButton
    {
        private const float DefaultSpeed = 1f;

        [SerializeField] private float _activeSpeed = 2f;

        public void OnEnable()
        {
            base.Init();

            SetNormalSettings();
        }

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            SetActiveSpeedSettings();
        }

        protected override void OnExitClick()
        {
            base.OnExitClick();

            SetNormalSettings();
        }

        private void SetNormalSettings()
        {
            Time.timeScale = DefaultSpeed;
        }

        private void SetActiveSpeedSettings()
        {
            Time.timeScale = _activeSpeed;
        }
    }
}