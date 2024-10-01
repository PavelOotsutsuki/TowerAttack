using GameFields.Persons.Discovers;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionViewLogic : DiscoverViewLogic
    {
        public override void View()
        {
            //float bigHeight = cardHeight * ScaleFactor;
            //float bigWidth = cardWidth * ScaleFactor;

            //RectTransform.sizeDelta = new Vector2(bigWidth, bigHeight);
            //RectTransform.localPosition = Vector3.zero;
            Movement.MoveLocalInstantly(Vector3.zero, Quaternion.identity.eulerAngles, ScaleFactor);


            gameObject.SetActive(true);
        }
    }
}
