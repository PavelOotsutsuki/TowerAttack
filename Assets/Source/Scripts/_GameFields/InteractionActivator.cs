using GameFields.Persons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.Hands;
using Tools;
using UnityEngine;

namespace GameFields
{
    public class InteractionActivator
    {
        private readonly ICardDragAndDropBlockable _dragAndDropBlockable;
        private readonly IWorkable _tower;
        private readonly IWorkable _table;
        private readonly IWorkable _endTurnButton;
        private readonly IBlockable _cardDragAndDropLightController;

        public InteractionActivator(ICardDragAndDropBlockable dragAndDropBlockable, IWorkable towerEnemy, IWorkable tablePlayer,
            IWorkable endTurnButton, IBlockable cardDragAndDropLightController)
        {
            _dragAndDropBlockable = dragAndDropBlockable;
            _tower = towerEnemy;
            _table = tablePlayer;
            _endTurnButton = endTurnButton;
            _cardDragAndDropLightController = cardDragAndDropLightController;
        }

        public void SetObjectsStates(PersonStep personStep)
        {
            //Debug.Log(personStep.ToString());

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
                case CardAttackProcessingPlayer:
                    SetCardActionProcessingPlayerStates();
                    break;
                case StartTurnDrawEnemyAI:
                    SetEnemyAIStates();
                    break;
                case EnemyDragAndDropImitation:
                case CardEffectProcessingEnemyAI:
                case CardAttackProcessingEnemyAI:
                    break;
                default:
                    throw new System.Exception("Неизветное состояние PersonStep" + personStep);
            }
        }

        private void SetStartPlayerTurnViewStates()
        {
            _dragAndDropBlockable.ForciblyBlock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
        }

        private void SetStartTurnDrawPlayerStates()
        {
            _dragAndDropBlockable.ForciblyBlock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
        }

        private void SetTurnProcessingStates()
        {
            _dragAndDropBlockable.Unblock();
            _tower.Activate();
            _table.Activate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Unblock();
        }

        private void SetCardActionProcessingPlayerStates()
        {
            _dragAndDropBlockable.ForciblyBlock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
        }

        private void SetEndTurnProcessingStates()
        {
            _dragAndDropBlockable.Unblock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Activate();
            _cardDragAndDropLightController.Block();
        }

        private void SetEnemyAIStates()
        {
            _dragAndDropBlockable.Unblock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
        }
    }
}
