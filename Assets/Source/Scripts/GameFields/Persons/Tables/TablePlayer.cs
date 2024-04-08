using Cards;
using GameFields.Effects;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class TablePlayer : Table, ICardDropPlayPlace
    {
        public override void Init(IUnbindCardManager unbindCardManager, EffectRoot effectRoot)
        {
            base.Init(unbindCardManager, effectRoot);

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
