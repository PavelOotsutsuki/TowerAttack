using Cards;
using GameFields.Effects;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class TablePlayer : Table, ICardDropPlace
    {
        public override void Init(IPlayCardManager playCardManager, EffectRoot effectRoot)
        {
            base.Init(playCardManager, effectRoot);

            FirstTurnDeactivate();
        }

        public void Activate()
        {
            if (gameObject.activeSelf == false)
            {
                gameObject.SetActive(true);
            }

            CanvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            CanvasGroup.blocksRaycasts = false;
        }

        private void FirstTurnDeactivate()
        {
            gameObject.SetActive(false);
            Deactivate();
        }
    }
}
