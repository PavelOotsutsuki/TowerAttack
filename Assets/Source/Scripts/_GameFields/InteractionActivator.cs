using System;
using GameFields.InputSettings;
using GameFields.Persons;
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

        private readonly GameFieldInputRoot _inputRoot;

        public InteractionActivator(ICardDragAndDropBlockable dragAndDropBlockable, IWorkable towerEnemy, IWorkable tablePlayer,
            IWorkable endTurnButton, IBlockable cardDragAndDropLightController, IWorkable forgingZone,
            IWorkable handTransferZone, GameFieldInputRoot inputRoot)
        {
            _dragAndDropBlockable = dragAndDropBlockable;
            _tower = towerEnemy;
            _table = tablePlayer;
            _endTurnButton = endTurnButton;
            _cardDragAndDropLightController = cardDragAndDropLightController;
            _forgingZone = forgingZone;
            _handTransferZone = handTransferZone;

            _inputRoot = inputRoot;
        }

        internal void SetObjectsStates(PersonStep personStep)
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
                case TurnProcessing turnProcessing:
                    _inputRoot.SetInputType(turnProcessing);
                    SetTurnProcessingStates();
                    break;
                //case CardEffectProcessingPlayer:
                //    SetCardActionProcessingPlayerStates();
                //    break;
                case EndTurnProcessing endTurnProcessing:
                    _inputRoot.SetInputType(endTurnProcessing);
                    SetEndTurnProcessingStates();
                    break;
                //case CardAttackProcessingPlayer:
                //    SetCardActionProcessingPlayerStates();
                //    break;
                case CardActionProcessingPlayer:
                    SetCardActionProcessingPlayerStates();
                    break;
                case StartTurnDrawEnemyAI:
                case EnemySkipTurnView:
                    SetEnemyAIStates();
                    break;
                case EnemyDragAndDropImitation enemyDragAndDropImitation:
                    _inputRoot.SetInputType(enemyDragAndDropImitation);
                    SetEnemyAIActionsStates();
                    break;
                case CardActionProcessingEnemyAI cardActionProcessingEnemyAI:
                    _inputRoot.SetInputType(cardActionProcessingEnemyAI);
                    SetEnemyAIActionsStates();
                    break;
                case OnBeforeEndTurnProcessing onBeforeEndTurnProcessing:
                    //case CardEffectProcessingEnemyAI:
                    //case CardAttackProcessingEnemyAI:
                    _inputRoot.SetInputType(onBeforeEndTurnProcessing);
                    SetEnemyAIActionsStates();
                    break;
                default:
                    throw new Exception("Неизвестное состояние PersonStep" + personStep);
            }
        }

        private void SetStartPlayerTurnViewStates()
        {
            _inputRoot.Pause();

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
            _inputRoot.Pause();

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
            _inputRoot.Pause();

            _dragAndDropBlockable.Unblock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
            _forgingZone.Deactivate();
            _handTransferZone.Deactivate();
            //Debug.Log("SetEnemyAIStates");
        }

        private void SetEnemyAIActionsStates()
        {
            _dragAndDropBlockable.Unblock();
            _tower.Deactivate();
            _table.Deactivate();
            _endTurnButton.Deactivate();
            _cardDragAndDropLightController.Block();
            _forgingZone.Deactivate();
            _handTransferZone.Deactivate();
        }
    }
}