using GameFields.InputSettings;
using Tools;
using Tools.InputSettings;

namespace GameFields.Persons
{
    public class CardActionProcessingEnemyAI : CardActionProcessing, IInputLogicObject
    {
        public CardActionProcessingEnemyAI(InteractionActivator interactionActivator, ICompletable completable) : base(interactionActivator, completable)
        { }
    }
}