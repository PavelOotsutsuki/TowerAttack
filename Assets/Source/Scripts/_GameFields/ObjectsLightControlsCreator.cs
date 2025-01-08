using System.Collections;
using System.Collections.Generic;
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

        [SerializeField] private CardAttackZoneEnemyAILightFrame _cardAttackZoneEnemyAI;
        [SerializeField] private CardPlayingZonePlayerLightFrame _cardPlayingZonePlayer;

        public void Init()
        {
            _lightPanel.Init();

            _cardAttackZoneEnemyAI.Init();
            _cardPlayingZonePlayer.Init();
        }

        public CardDragAndDropLightController CreateDragAndDropLightController()
        {
            return new CardDragAndDropLightController(_lightPanel, _cardAttackZoneEnemyAI, _cardPlayingZonePlayer);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(ObjectsLightControlsCreator))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineLightPanel(),
                DefineCardAttackZoneEnemyAILightFrame(),
                DefineCardPlayingZonePlayerLightFrame()
            };

            return list;
        }

        [ContextMenu(nameof(DefineLightPanel))]
        private ComponentAttachInfo DefineLightPanel()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _lightPanel, ComponentLocationTypes.InChildren);
        }

        [ContextMenu(nameof(DefineCardAttackZoneEnemyAILightFrame))]
        private ComponentAttachInfo DefineCardAttackZoneEnemyAILightFrame()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardAttackZoneEnemyAI, ComponentLocationTypes.InScene);
        }

        [ContextMenu(nameof(DefineCardPlayingZonePlayerLightFrame))]
        private ComponentAttachInfo DefineCardPlayingZonePlayerLightFrame()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _cardPlayingZonePlayer, ComponentLocationTypes.InScene);
        }
        #endregion
    }
}
