using System.Collections;
using System.Collections.Generic;
using Cards;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Hands
{
    public class HandAI_old : Hand
    {
        private const float SortDirection = -1;

        public override void Init()
        {
            base.Init();
            CanvasGroup.blocksRaycasts = true;
        }

        public void CardDragAnimation()
        {
            // NEW!
            // тут типо будет анимация лже-драга
            // замена метода OnCardDrag
            // ну или в отдельном классе
        }

        public void CardDropAnimation()
        {
            // NEW!
            // тут типо будет анимация лже-дропа после драга
            // замена метода OnCardDrop
            // ну или в отдельном классе
        }

        protected override float GetSortDirection()
        {
            return SortDirection;
        }
    }
}
