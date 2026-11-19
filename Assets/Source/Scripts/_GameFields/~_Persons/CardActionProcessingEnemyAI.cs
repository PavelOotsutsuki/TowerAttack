using System.Threading;
using Tools;
using Tools.InputSettings;

namespace GameFields.Persons
{
    internal class CardActionProcessingEnemyAI : CardActionProcessing, IInputLogicObject
    {
        public CardActionProcessingEnemyAI(InteractionActivator interactionActivator, ICompletable completable, CancellationToken token) :
            base(interactionActivator, completable, token)
        { }
    }
}