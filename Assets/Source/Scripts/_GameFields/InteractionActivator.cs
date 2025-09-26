using GameFields.Persons.Commons;
using GameFields.Persons.DrawCards;
using GameFields.Persons.EnemyProcessImitations;
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
        private readonly IWorkable _forgingZone;
        private readonly IWorkable _handTransferZone;

        public InteractionActivator(ICardDragAndDropBlockable dragAndDropBlockable, IWorkable towerEnemy, IWorkable tablePlayer,
            IWorkable endTurnButton, IBlockable cardDragAndDropLightController, IWorkable forgingZone,
            IWorkable handTransferZone)
        {
            _dragAndDropBlockable = dragAndDropBlockable;
            _tower = towerEnemy;
            _table = tablePlayer;
            _endTurnButton = endTurnButton;
            _cardDragAndDropLightController = cardDragAndDropLightController;
            _forgingZone = forgingZone;
            _handTransferZone = handTransferZone;
        }

        public void SetObjectsStates(PersonStep personStep)
        {
            //Debug.Log(personStep.ToString());

            switch (personStep)
            {
                case StartPlayerTurnView:
                case PlayerSkipTurnView:
                    SetStartPlayerTurnViewStates();
                    break;
                case StartTurnDrawPlayer:
                    SetStartTurnDrawPlayerStates();
                    break;
                case TurnProcessing:
                    SetTurnProcessingStates();
                    break;
                //case CardEffectProcessingPlayer:
                //    SetCardActionProcessingPlayerStates();
                //    break;
                case EndTurnProcessing:
                    SetEndTurnProcessingStates();
                    break;
                //case CardAttackProcessingPlayer:
                //    SetCardActionProcessingPlayerStates();
                //    break;
                case CardActionProcessingPlayer:
                    SetCardActionProcessingPlayerStates();
                    break;
                case StartTurnDrawEnemyAI:
                    SetEnemyAIStates();
                    break;
                case EnemyDragAndDropImitation:
                case CardActionProcessingEnemyAI:
                case OnBeforeEndTurnProcessing:
                case EnemySkipTurnView:
                //case CardEffectProcessingEnemyAI:
                    //case CardAttackProcessingEnemyAI:
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
            _forgingZone.Deactivate();
            _handTransferZone.Deactivate();
            //Debug.Log("SetStartPlayerTurnViewStates");
        }

        private void SetStartTurnDrawPlayerStates()
        {
            _dragAndDropBlockable.ForciblyBlock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
            _forgingZone.Deactivate();
            _handTransferZone.Deactivate();
            //Debug.Log("SetStartTurnDrawPlayerStates");

        }

        private void SetTurnProcessingStates()
        {
            _dragAndDropBlockable.Unblock();
            _tower.Activate();
            _table.Activate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Unblock();
            _forgingZone.Activate();
            _handTransferZone.Activate();
            //Debug.Log("SetTurnProcessingStates");
        }

        private void SetCardActionProcessingPlayerStates()
        {
            _dragAndDropBlockable.ForciblyBlock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
            _forgingZone.Deactivate();
            _handTransferZone.Deactivate();
            //Debug.Log("SetCardActionProcessingPlayerStates");
        }

        private void SetEndTurnProcessingStates()
        {
            _dragAndDropBlockable.Unblock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Activate();
            _cardDragAndDropLightController.Block();
            _forgingZone.Deactivate();
            _handTransferZone.Deactivate();
            //Debug.Log("SetEndTurnProcessingStates");
        }

        private void SetEnemyAIStates()
        {
            _dragAndDropBlockable.Unblock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
            _forgingZone.Deactivate();
            _handTransferZone.Deactivate();
            //Debug.Log("SetEnemyAIStates");
        }
    }
}
