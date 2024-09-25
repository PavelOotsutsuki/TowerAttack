using GameFields.Persons.Discovers;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionViewLogic : DiscoverViewLogic
    {
        public override void View(float cardHeight, float cardWidth)
        {
            float bigHeight = cardHeight * ScaleFactor;
            float bigWidth = cardWidth * ScaleFactor;

            RectTransform.sizeDelta = new Vector2(bigWidth, bigHeight);
            RectTransform.localPosition = Vector3.zero;

            gameObject.SetActive(true);
        }
    }
}
