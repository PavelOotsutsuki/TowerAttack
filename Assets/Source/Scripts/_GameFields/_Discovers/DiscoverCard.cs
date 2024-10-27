using System;
using Cards;
using UnityEngine;
using UnityEngine.UI;
using Tools;

namespace GameFields.Persons.Discovers
{
    public abstract class DiscoverCard : MonoBehaviour, IDeactivatable, IWorkable<DiscoverCardActivateData>
    {
        [SerializeField, Min(0f)] protected float ViewDuration = 0.5f;
        [SerializeField] protected DiscoverViewLogic ViewLogic;

        protected Action ClickCallback;
        protected IDiscoverClickHandler _discoverClickHandler;

        public virtual void Init(Action clickCallback, IDiscoverClickHandler discoverClickHandler)
        {
            ViewLogic.Init(ViewDuration);
            _discoverClickHandler = discoverClickHandler;
            ClickCallback = clickCallback;

            Deactivate();
        }

        public abstract void Deactivate();
        public abstract void Activate(DiscoverCardActivateData data);
        //public abstract void Activate(CardViewConfig cardViewConfig = null);
        public abstract void StartClickActions();
    }
}
