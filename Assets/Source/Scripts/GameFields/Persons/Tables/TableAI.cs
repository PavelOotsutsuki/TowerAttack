using Cards;
using GameFields.Effects;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class TableAI : Table, ICardDropPlaceImitation
    {
        public override void Init(IPlayCardManager playCardManager, EffectRoot effectRoot)
        {
            base.Init(playCardManager, effectRoot);

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
