using GameFields.Persons.Discovers;
using UnityEngine;

namespace GameFields.StartFights
{
    public class StartTowerCardSelectionViewLogic : DiscoverViewLogic
    {
        public override void Show(DiscoverViewLogicData data)
        {
            RectTransform.sizeDelta = new Vector2(data.CardWidth, data.CardHeight);
            RectTransform.localScale = new Vector3(1, 1, 1);

            Movement.MoveLocalInstantly(Vector3.zero, Quaternion.identity.eulerAngles, ScaleFactor);

            IsComplete = true;
        }
    }
}