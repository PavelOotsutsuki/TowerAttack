using Tools.UI.UIHelpers;

namespace GameFields.HistoryMenues
{
    public class HistoryMenuActivateButton : MenuActivateButton
    {
        public void Init(HistoryMenu historyMenu, UIHelperDescription UIHelperDescription)
        {
            base.Init(historyMenu, UIHelperDescription);
        }

        protected override string GetHelperText()
        {
            return "История";
        }
    }
}