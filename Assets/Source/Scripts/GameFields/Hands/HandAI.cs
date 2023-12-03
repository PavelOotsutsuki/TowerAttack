using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFields.Hands
{
    public class HandAI : Hand
    {
        //protected override float StartRotation => 0.5f;

        //protected override void SortHandSeats()
        //{
        //    if (HandSeats.Count <= 0)
        //    {
        //        return;
        //    }

        //    float offsetX;
        //    float positionX;

        //    if (HandSeats.Count * _offsetX < _handLength / 2)
        //    {
        //        offsetX = _offsetX;
        //    }
        //    else
        //    {
        //        float xFactor = _offsetX * HandSeats.Count;
        //        float fullOffsetX = _handLength * xFactor / (xFactor + _handLength / 2);

        //        offsetX = fullOffsetX / HandSeats.Count;
        //    }

        //    for (int i = 0; i < HandSeats.Count; i++)
        //    {
        //        positionX = _startPositionX + ((HandSeats.Count - 1) / 2f - i) * offsetX * -1;
        //        Vector3 positon = new Vector2(positionX + RectTransform.rect.xMin, StartPositionY + RectTransform.rect.yMin);
        //        Vector3 rotation = new Vector3(0f, 0f, StartRotation);
        //        HandSeats[i].SetLocalPositionValues(positon, rotation, StartCardTranslateSpeed);
        //    }
        //}
    }
}
