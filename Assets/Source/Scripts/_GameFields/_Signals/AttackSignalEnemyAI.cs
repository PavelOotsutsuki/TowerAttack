using Tools;

namespace GameFields.Signals
{
    public struct AttackSignalEnemyAI
    {
        public readonly ICompletable Completable;

        public AttackSignalEnemyAI(ICompletable completable)
        {
            Completable = completable;
        }
    }
}