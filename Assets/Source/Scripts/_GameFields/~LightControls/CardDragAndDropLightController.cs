using System.Collections.Generic;
using Cards;
using Cards.Views;
using Tools;

namespace GameFields.LightControls
{
    public class CardDragAndDropLightController: IWorkable<CardDragAndDropLightControllerActivateData>, IBlockable
    {
        private readonly LightableObject _cardAttackZoneEnemyAI;
        private readonly LightableObject _cardPlayingZonePlayer;
        private readonly LightableObject _forgingLightableObject;
        private readonly LightableObject _handTransferLightableObject;

        private readonly LightController _lightController;

        private List<LightableObject> _currentObjects = new List<LightableObject>();

        public CardDragAndDropLightController(LightController lightController, LightableObject cardAttackZoneEnemyAI,
            LightableObject cardPlayingZonePlayer, LightableObject forgingLightableObject, LightableObject handTransferLightableObject)
        {
            _lightController = lightController;

            _cardAttackZoneEnemyAI = cardAttackZoneEnemyAI;
            _cardPlayingZonePlayer = cardPlayingZonePlayer;
            _forgingLightableObject = forgingLightableObject;
            _handTransferLightableObject = handTransferLightableObject;
        }

        public bool? IsActive { get; private set; } = null;

        public void Activate(CardDragAndDropLightControllerActivateData data)
        {
            if (IsActive == true)
                return;

            IsActive = true;

            _currentObjects.Clear();

            if ((data.EffectFeature & CardCapability.Attack) == CardCapability.Attack)
                _currentObjects.Add(_cardAttackZoneEnemyAI);

            if ((data.EffectFeature & CardCapability.Play) == CardCapability.Play)
                _currentObjects.Add(_cardPlayingZonePlayer);

            if ((data.EffectFeature & CardCapability.GnomeForging) == CardCapability.GnomeForging)
                _currentObjects.Add(_forgingLightableObject);

            if ((data.EffectFeature & CardCapability.HandTransfer) == CardCapability.HandTransfer)
                _currentObjects.Add(_handTransferLightableObject);

            LightControllerActivateData lightControllerActivateData = new LightControllerActivateData(_currentObjects);
            _lightController.Activate(lightControllerActivateData);
        }

        public void Deactivate()
        {
            if (IsActive == false)
                return;

            IsActive = false;

            _lightController?.Deactivate();
        }

        public void Block()
        {
            _lightController.Block();
        }

        public void Unblock()
        {
            _lightController.Unblock();
        }
    }
}