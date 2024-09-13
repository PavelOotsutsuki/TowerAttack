using System;
using Cards;
using Tools;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverCard : MonoBehaviour
    {
        [SerializeField, Min(0.1f)] protected float GrowDuration = 1f;
        [SerializeField] protected Image FrameImage;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField, Min(1f)] private float _scaleFactor = 2f;

        protected Action ClickCallback;
        protected IDiscoverClickHandler _discoverClickHandler;

        private Movement _movement;

        public virtual void Init(Action clickCallback, IDiscoverClickHandler discoverClickHandler)
        {
            _discoverClickHandler = discoverClickHandler;
            _rectTransform.rotation = Quaternion.identity;
            _rectTransform.localPosition = Vector3.zero;
            ClickCallback = clickCallback;
            _movement = new Movement(_rectTransform);

            Hide();
        }

        public abstract void Hide();
        public abstract void Activate(float cardHeight, float cardWidth, CardViewConfig cardViewConfig = null);
        public abstract void StartClickActions();

        protected void View(float cardHeight, float cardWidth)
        {
            float bigHeight = cardHeight * _scaleFactor;
            float bigWidth = cardWidth * _scaleFactor;

            _rectTransform.sizeDelta = new Vector2(bigWidth, bigHeight);
            _rectTransform.localPosition = Vector3.zero;

            Vector3 defaultScale = _rectTransform.localScale;
            _movement.MoveLocalInstantly(Vector3.zero, Quaternion.identity.eulerAngles, Vector3.zero);
            _movement.MoveLocalSmoothly(Vector3.zero, Quaternion.identity.eulerAngles, GrowDuration, defaultScale);

            gameObject.SetActive(true);
        }

        #region AutomaticFillComponents
        [ContextMenu(nameof(DefineAllComponents))]
        private void DefineAllComponents()
        {
            DefineRectTransform();
        }

        [ContextMenu(nameof(DefineRectTransform))]
        private void DefineRectTransform()
        {
            AutomaticFillComponents.DefineComponent(this, ref _rectTransform, ComponentLocationTypes.InThis);
        }
        #endregion 
    }
}
