using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

namespace GameFields.LightControls
{
    public class CardDragAndDropLightController: LightController
    {
        private readonly IViewable _cardAttackZoneEnemyAI;
        private readonly IViewable _cardPlayingZonePlayer;

        public CardDragAndDropLightController(LightPanel lightPanel, LightFrame cardAttackZoneEnemyAI,
            LightFrame cardPlayingZonePlayer) : base(lightPanel)
        {
            _cardAttackZoneEnemyAI = cardAttackZoneEnemyAI;
            _cardPlayingZonePlayer = cardPlayingZonePlayer;
        }

        protected override void OnActivate()
        {
            _cardAttackZoneEnemyAI.Show();
            _cardPlayingZonePlayer.Show();
        }

        protected override void OnDeactivate()
        {
            _cardAttackZoneEnemyAI.Hide();
            _cardPlayingZonePlayer.Hide();
        }
    }
}