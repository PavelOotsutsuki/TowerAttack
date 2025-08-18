using Tools;

namespace GameFields.Signals
{
    public struct AttackSignalPlayer
    {
        public readonly ICompletable Completable;

        public AttackSignalPlayer(ICompletable completable)
        {
            Completable = completable;
        }
    }
}