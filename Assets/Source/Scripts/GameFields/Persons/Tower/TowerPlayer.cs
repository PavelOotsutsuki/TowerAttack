using Cards;
using UnityEngine;

namespace GameFields.Persons.Towers
{
    internal class TowerPlayer : Tower, ICardDropPlace
    {
        private IStartFightListener _startFightListener;

        public override void Init(IPlayCardManager playCardManager)
        {
            base.Init(playCardManager);

            Activate();
        }

        public void SetStartFightListener(IStartFightListener startFightListener)
        {
            _startFightListener = startFightListener;
        }

        public bool TrySeatCard(Card card)
        {
            if (TowerSeat.TryGetCard(card))
            {
                PlayCardManager.PlayCard(card);
                _startFightListener.StartFight();

                Deactivate();

                return true;
            }

            return false;
        }

        public void Activate()
        {
            if (TowerSeat.IsVoid())
            {
                CanvasGroup.blocksRaycasts = true;
            }
        }
    }
}
