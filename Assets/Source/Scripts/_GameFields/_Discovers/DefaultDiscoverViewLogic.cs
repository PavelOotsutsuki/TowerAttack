using Tools;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public class DefaultDiscoverViewLogic : DiscoverViewLogic
    {
        public override void View(float cardHeight, float cardWidth)
        {
            //float bigHeight = cardHeight * ScaleFactor;
            //float bigWidth = cardWidth * ScaleFactor;

            RectTransform.sizeDelta = new Vector2(cardWidth, cardHeight);
            RectTransform.localPosition = Vector3.zero;

            Vector3 endScale = new Vector3(1,1,1) * ScaleFactor;
            Movement.MoveLocalInstantly(Vector3.zero, Quaternion.identity.eulerAngles, Vector3.zero);
            Movement.MoveLocalSmoothly(Vector3.zero, Quaternion.identity.eulerAngles, Duration, endScale);

            gameObject.SetActive(true);
        }
    }
}
