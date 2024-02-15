using Cards;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    internal class TablePlayer : Table, ICardDropPlace
    {
        public override void Init(IPlayCardManager playCardManager)
        {
            base.Init(playCardManager);

            Deactivate();
        }

        public void Activate()
        {
            CanvasGroup.blocksRaycasts = true;
        }

        public void Deactivate()
        {
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
