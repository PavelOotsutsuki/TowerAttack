using Cards;
using GameFields.Effects;
using GameFields.Persons.Hands;
using UnityEngine;

namespace GameFields.Persons.Tables
{
    public class TableAI : Table, ICardPlayPlaceImitation
    {
        public override void Init(IUnbindCardManager playCardManager, EffectRoot effectRoot)
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
