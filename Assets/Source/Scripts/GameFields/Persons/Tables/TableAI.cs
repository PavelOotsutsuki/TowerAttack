using Cards;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    internal class TableAI : Table, ICardDropPlaceImitation
    {
        public override void Init(IPlayCardManager playCardManager, CardEffects cardEffects)
        {
            base.Init(playCardManager, cardEffects);

            Deactivate();
        }

        public Vector3 GetCentralСoordinates()
        {
            return transform.position;
        }

        private void Deactivate()
        {
            CanvasGroup.blocksRaycasts = false;
        }
    }
}
