using System.Threading;
using Tools.UI.UIHelpers;

namespace GameFields.FightMenues
{
    public class FightMenuActivateButton : MenuActivateButton
    {
        public void Init(FightMenu fightMenu, UIHelperDescription UIHelperDescription, CancellationToken gameFieldToken)
        {
            base.Init(fightMenu, UIHelperDescription, gameFieldToken);
        }

        protected override string GetHelperText()
        {
            return "Меню";
        }
    }
}