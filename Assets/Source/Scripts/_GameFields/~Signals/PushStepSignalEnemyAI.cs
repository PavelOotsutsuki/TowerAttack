using Tools;

namespace GameFields.Signals
{
    public struct PushStepSignalEnemyAI
    {
        public readonly ICompletable Completable;

        public PushStepSignalEnemyAI(ICompletable completable)
        {
            Completable = completable;
        }
    }
}