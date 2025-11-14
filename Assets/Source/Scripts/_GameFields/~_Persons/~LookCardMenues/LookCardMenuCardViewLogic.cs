using System.Collections.Generic;
using Tools;
using Tools.Utils.FillComponents;
using Tools.Utils.Movements;
using UnityEngine;

namespace GameFields.Persons.LookCardMenues
{
    public class LookCardMenuCardViewLogic : MonoBehaviour, ICompletable, IShowable<LookCardMenuCardViewLogicData>, IAutomaticFillComponents
    {
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField, Min(1f)] private float _scaleFactor = 2f;

        private float _duration;
        private Movement _movement;

        public bool IsComplete { get; protected set; }

        public void Init(float duration)
        {
            IsComplete = false;
            _duration = duration;
            _rectTransform.rotation = Quaternion.identity;
            _rectTransform.localPosition = Vector3.zero;
            _movement = new Movement(_rectTransform);
        }

        public void Show(LookCardMenuCardViewLogicData data)
        {
            IsComplete = false;

            _rectTransform.sizeDelta = new Vector2(data.CardWidth, data.CardHeight);
            _rectTransform.localPosition = Vector3.zero;
            //Debug.Log("_rectTransform.sizeDelta: " + _rectTransform.sizeDelta);

            Vector3 endScale = new Vector3(1, 1, 1) * _scaleFactor;
            //Debug.Log("endScale: " + endScale);
            _movement.MoveLocalInstantly(Vector3.zero, Quaternion.identity.eulerAngles, Vector3.zero);
            _movement.MoveLocalSmoothly(Vector3.zero, Quaternion.identity.eulerAngles, _duration, endScale, () => IsComplete = true);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents) + nameof(LookCardMenuCardViewLogic))]
        public List<ComponentAttachInfo> DefineAllComponents()
        {
            List<ComponentAttachInfo> list = new List<ComponentAttachInfo>
            {
                DefineRectTransform()
            };

            return list;
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private ComponentAttachInfo DefineRectTransform()
        {
            return AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}