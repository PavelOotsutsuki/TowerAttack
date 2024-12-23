using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using Tools;

namespace GameFields
{
    public class InteractionActivator
    {
        private readonly IHandBlockable _hand;
        private readonly IWorkable _tower;
        private readonly IWorkable _table;
        private readonly IWorkable _endTurnButton;

        public InteractionActivator(IHandBlockable handPlayer, IWorkable towerEnemy, IWorkable tablePlayer,
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
                case StartPlayerTurnView:
                    SetStartPlayerTurnViewStates();
                    break;
                case StartTurnDrawPlayer:
                    SetStartTurnDrawPlayerStates();
                    break;
                case TurnProcessing:
                    SetTurnProcessingStates();
                    break;
                case CardEffectProcessingPlayer:
                    SetCardActionProcessingPlayerStates();
                    break;
                case EndTurnProcessing:
                    SetEndTurnProcessingStates();
                    break;
                case StartTurnDrawEnemyAI:
                    SetEnemyAIStates();
                    break;
                case EnemyDragAndDropImitation:
                    SetEnemyAIStates();
                    break;
                case CardEffectProcessingEnemyAI:
                    SetEnemyAIStates();
                    break;
                case CardAttackProcessing:
                    SetCardActionProcessingPlayerStates();
                    break;
                default:
                    throw new System.Exception("Неизветное состояние PersonStep" + personStep);
            }
        }

        private void SetStartPlayerTurnViewStates()
        {
            _hand.ForciblyBlock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
        }

        private void SetStartTurnDrawPlayerStates()
        {
            _hand.ForciblyBlock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
        }

        private void SetTurnProcessingStates()
        {
            _hand.Unblock();
            _tower.Activate();
            _table.Activate();
            _endTurnButton.Deactivate();
        }

        private void SetCardActionProcessingPlayerStates()
        {
            _hand.ForciblyBlock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
        }

        private void SetEndTurnProcessingStates()
        {
            _hand.Unblock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Activate();
        }

        private void SetEnemyAIStates()
        {
            _hand.Unblock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
        }
    }
}
