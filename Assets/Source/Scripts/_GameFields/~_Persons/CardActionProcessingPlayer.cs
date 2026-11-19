using System.Threading;
using Tools;

namespace GameFields.Persons
{
    internal class CardActionProcessingPlayer : CardActionProcessing
    {
        public CardActionProcessingPlayer(InteractionActivator interactionActivator, ICompletable completable, CancellationToken token) :
            base(interactionActivator, completable, token)
        { }
    }
}