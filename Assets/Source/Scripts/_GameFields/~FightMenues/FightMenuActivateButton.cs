using Tools.UI.UIHelpers;

namespace GameFields.FightMenues
{
    public class FightMenuActivateButton : MenuActivateButton
    {
        public void Init(FightMenu fightMenu, UIHelperDescription UIHelperDescription)
        {
            base.Init(fightMenu, UIHelperDescription);
        }

        protected override string GetHelperText()
        {
            return "Меню";
        }
    }
}