using Tools;

namespace GameFields.Signals
{
    public struct PushStepSignalPlayer
    {
        public readonly ICompletable Completable;

        public PushStepSignalPlayer(ICompletable completable)
        {
            Completable = completable;
        }
    }
}