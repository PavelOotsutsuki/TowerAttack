using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace GameFields.LightControls
{
    public class ObjectsLightControlsCreator: MonoBehaviour
    {
        [SerializeField] private LightPanel _lightPanel;

        [SerializeField] private LightFrame _cardAttackZoneEnemyAI;
        [SerializeField] private LightFrame _cardPlayingZonePlayer;

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
    }
}
