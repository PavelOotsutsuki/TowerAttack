using System;
using Cards;
using UnityEngine;
using UnityEngine.UI;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverCard : MonoBehaviour
    {
        [SerializeField, Min(0f)] protected float ViewDuration = 0.5f;
        [SerializeField] protected Image FrameImage;
        [SerializeField] protected DiscoverViewLogic ViewLogic;

        protected Action ClickCallback;
        protected IDiscoverClickHandler _discoverClickHandler;

        public virtual void Init(Action clickCallback, IDiscoverClickHandler discoverClickHandler)
        {
            ViewLogic.Init(ViewDuration);
            _discoverClickHandler = discoverClickHandler;
            ClickCallback = clickCallback;

            Hide();
        }

        public abstract void Hide();
        //public abstract void Activate(float cardHeight, float cardWidth, CardViewConfig cardViewConfig = null);
        public abstract void Activate(CardViewConfig cardViewConfig = null);
        public abstract void StartClickActions();
    }
}
