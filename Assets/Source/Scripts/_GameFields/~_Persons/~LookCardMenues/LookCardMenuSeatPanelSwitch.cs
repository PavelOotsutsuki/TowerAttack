using System;
using Tools.UI;

namespace GameFields.Persons.LookCardMenues
{
    public abstract class LookCardMenuSeatPanelSwitch : ConfirmableButton
    {
        private Action _onEnterClick;

        public void Init(Action onEnterClick)
        {
            base.Init();

            _onEnterClick = onEnterClick;

            BaseDeactivate();
        }

        public void Activate()
        {
            BaseActivate();
        }

        public void Deactivate()
        {
            BaseDeactivate();
        }

        protected override void OnEnterClick()
        {
            _onEnterClick?.Invoke();

            IsClicked = false;
            CanvasGroup.blocksRaycasts = true;
        }
    }
}