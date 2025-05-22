using System.Collections;
using System.Collections.Generic;
using GameFields.DiscardPiles;
using GameFields.Persons.Tables;
using GameFields.Persons.Towers;
using Tools;
using Tools.Utils.FillComponents;
using UnityEngine;

namespace GameFields.LightControls
{
    public class ObjectsLightControlsCreator: MonoBehaviour, IAutomaticFillComponents
    {
        [SerializeField] private LightPanel _lightPanel;

        [SerializeField] private CardAttackZoneEnemyAILightableObject _cardAttackZoneEnemyAI;
        [SerializeField] private CardPlayingZonePlayerLightableObject _cardPlayingZonePlayer;
        [SerializeField] private ForgingLightableObject _forgingLightableObject;
        [SerializeField] private float _dragAndDropDelayForActivate = 3f;

        public void Init()
        {
            _lightPanel.Init();

            _cardAttackZoneEnemyAI.Init();
            _cardPlayingZonePlayer.Init();
            _forgingLightableObject.Init();
        }

        public CardDragAndDropLightController CreateCardDragAndDropLightController()
        {
            LightableObject[] defaultLightableObjects = new LightableObject[]
            {
                _cardAttackZoneEnemyAI,
                _cardPlayingZonePlayer
            };

            LightController defaultLightController = new LightController(_lightPanel, defaultLightableObjects, _dragAndDropDelayForActivate);

            LightableObject[] gnomeLightableObjects = new LightableObject[]
            {
                _cardAttackZoneEnemyAI,
                _cardPlayingZonePlayer,
                _forgingLightableObject
            };

            LightController gnomeLightController = new LightController(_lightPanel, gnomeLightableObjects, _dragAndDropDelayForActivate);

            return new CardDragAndDropLightController(defaultLightController, gnomeLightController);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ObjectsLightControlsCreator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLightPanel(),
                DefineCardAttackZoneEnemyAILightableObject(),
                DefineCardPlayingZonePlayerLightableObject(),
                DefineForgingLightableObject()
            };

            return list;
        }

        [ContextMenu(nameof(DefineLightPanel))]
        private ComponentAttachInfo DefineLightPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lightPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardAttackZoneEnemyAILightableObject))]
        private ComponentAttachInfo DefineCardAttackZoneEnemyAILightableObject()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardAttackZoneEnemyAI, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineCardPlayingZonePlayerLightableObject))]
        private ComponentAttachInfo DefineCardPlayingZonePlayerLightableObject()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardPlayingZonePlayer, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineForgingLightableObject))]
        private ComponentAttachInfo DefineForgingLightableObject()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _forgingLightableObject, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}