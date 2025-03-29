using Tools;

namespace GameFields.Signals
{
    public struct AttackSignal
    {
        public readonly ICompletable Completable;

        public AttackSignal(ICompletable completable)
        {
            Completable = completable;
        }
    }
}