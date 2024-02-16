using Cards;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    internal class TablePlayer : Table, ICardDropPlace
    {
        public override void Init(IPlayCardManager playCardManager)
        {
            base.Init(playCardManager);

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
