using System;
using Tools;
using Tools.UI;

namespace GameFields.FightMenues
{
    public class FightMenuActivateButton : ConfirmableButton
    {
        private IActivatable _fightMenu;

        public void Init(IActivatable fightMenu)
        {
            base.Init();

            _fightMenu = fightMenu;

            Deactivate();
        }

        protected override void OnEnterClick()
        {
            _fightMenu.Activate();

            IsClicked = false;
            CanvasGroup.blocksRaycasts = true;
        }
    }
}