using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using Tools;

namespace GameFields
{
    public class GameFieldObjectsActivator
    {
        private readonly IHandBlockable _hand;
        private readonly IWorkable _tower;
        private readonly IWorkable _table;
        private readonly IWorkable _endTurnButton;

        public GameFieldObjectsActivator(IHandBlockable handPlayer, IWorkable towerEnemy, IWorkable tablePlayer,
            IWorkable endTurnButton)
        {
            _hand = handPlayer;
            _tower = towerEnemy;
            _table = tablePlayer;
            _endTurnButton = endTurnButton;
        }

        public void SetObjectsStates(PersonStep personStep)
        {
            switch (personStep)
            {
                case TurnProcessing:
                    break;
                case CardEffectProcessing:
                    break;
                case EndTurnProcessing:
                    break;
                case EnemyDragAndDropImitation:
                    break;
                case StartPlayerTurnView:
                    break;
                case StartTurnDrawPlayer:
                    break;
                case StartTurnDrawEnemyAI:
                    break;
            }
        }


    }
}
