using GameFields.Persons.Discovers;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionViewLogic : DiscoverViewLogic
    {
        public override void View(float cardHeight, float cardWidth)
        {
            //float bigHeight = cardHeight * ScaleFactor;
            //float bigWidth = cardWidth * ScaleFactor;

            RectTransform.sizeDelta = new Vector2(cardWidth, cardHeight);
            RectTransform.localScale = new Vector3(1, 1, 1);
            //RectTransform.localPosition = Vector3.zero;
            Movement.MoveLocalInstantly(Vector3.zero, Quaternion.identity.eulerAngles, ScaleFactor);


            gameObject.SetActive(true);
        }
    }
}
