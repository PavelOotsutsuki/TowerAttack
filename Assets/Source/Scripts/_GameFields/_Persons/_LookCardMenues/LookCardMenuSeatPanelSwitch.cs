using System;
using Tools.UI;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuSeatPanelSwitch : ConfirmableButton
    {
        private Action _onEnterClick;

        public void Init(Action onEnterClick)
        {
            base.Init();

            _onEnterClick = onEnterClick;

            Deactivate();
        }

        protected override void OnEnterClick()
        {
            base.OnEnterClick();

            _onEnterClick?.Invoke();

            IsClicked = false;
            CanvasGroup.blocksRaycasts = true;
        }
    }
}