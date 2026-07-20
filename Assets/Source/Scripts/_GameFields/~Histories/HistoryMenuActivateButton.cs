using System.Threading;
using Tools.UI.UIHelpers;

namespace GameFields.Histories
{
    public class HistoryMenuActivateButton : MenuActivateButton
    {
        public void Init(HistoryMenu historyMenu, UIHelperDescription UIHelperDescription, CancellationToken gameFieldToken)
        {
            base.Init(historyMenu, UIHelperDescription, gameFieldToken);
        }

        protected override string GetHelperText()
        {
            return "История";
        }
    }
}