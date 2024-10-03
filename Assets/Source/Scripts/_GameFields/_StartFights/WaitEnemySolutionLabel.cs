using Tools;

namespace GameFields.StartFights
{
    public class WaitEnemySolutionLabel : FadableLabel
    {
        public bool IsWasStarted { get; private set; }

        public override void Show()
        {
            IsWasStarted = true;

            base.Show();
        }

        public override void Hide()
        {
            if (IsWasStarted == false)
                return;

            base.Hide();

            IsWasStarted = false;
        }
    }
}
