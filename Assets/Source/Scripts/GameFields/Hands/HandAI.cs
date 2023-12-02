using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Hands
{
    public class HandAI : Hand
    {
        protected override void SortHandSeats()
        {
            if (HandSeats.Count <= 0)
            {
                return;
            }

            float offsetX;
            float positionX;

            if (HandSeats.Count * OffsetX < HandLength / 2)
            {
                offsetX = OffsetX;
            }
            else
            {
                float xFactor = OffsetX * HandSeats.Count;
                float fullOffsetX = HandLength * xFactor / (xFactor + HandLength / 2);

                offsetX = fullOffsetX / HandSeats.Count;
            }

            for (int i = 0; i < HandSeats.Count; i++)
            {
                positionX = StartPositionX + ((HandSeats.Count - 1) / 2f - i) * offsetX * -1;
                Vector3 positon = new Vector2(positionX + RectTransform.rect.xMin, StartPositionY + RectTransform.rect.yMin);
                Vector3 rotation = new Vector3(0f, 0f, StartRotation);
                HandSeats[i].SetLocalPositionValues(positon, rotation, StartCardTranslateSpeed);
            }
        }
    }
}
