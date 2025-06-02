using System.Collections.Generic;
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
        [SerializeField] private HandTransferLightableObject _handTransferLightableObject;
        [SerializeField] private float _dragAndDropDelayForActivate = 3f;

        public void Init()
        {
            _lightPanel.Init();

            _cardAttackZoneEnemyAI.Init();
            _cardPlayingZonePlayer.Init();
            _forgingLightableObject.Init();
            _handTransferLightableObject.Init();
        }

        public CardDragAndDropLightController CreateCardDragAndDropLightController()
        {
            LightController lightController = new LightController(_lightPanel, _dragAndDropDelayForActivate);

            return new CardDragAndDropLightController(lightController, _cardAttackZoneEnemyAI, _cardPlayingZonePlayer,
                _forgingLightableObject, _handTransferLightableObject);
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
                DefineForgingLightableObject(),
                DefineHandTransferLightableObject()
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

        [ContextMenu(nameof(DefineHandTransferLightableObject))]
        private ComponentAttachInfo DefineHandTransferLightableObject()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _handTransferLightableObject, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}