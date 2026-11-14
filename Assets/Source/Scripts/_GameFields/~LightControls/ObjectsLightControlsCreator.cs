using System.Collections.Generic;
using System.Threading;
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

        private CancellationToken _fightToken;

        public void Init(CancellationToken fightToken)
        {
            _fightToken = fightToken;

            _lightPanel.Init(fightToken);

            _cardAttackZoneEnemyAI.Init(fightToken);
            _cardPlayingZonePlayer.Init(fightToken);
            _forgingLightableObject.Init(fightToken);
            _handTransferLightableObject.Init(fightToken);
        }

        public CardDragAndDropLightController CreateCardDragAndDropLightController()
        {
            LightController lightController = new LightController(_lightPanel, _dragAndDropDelayForActivate, _fightToken);

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