using Cards;
using Cards.Views;
using Tools;
using UnityEngine;

namespace GameFields.Persons.Discovers
{
    public class DiscoverCardActivateData: IData
    {
        private readonly Vector2 _sizeDelta;
        private readonly IDiscoverable _discoverable;

        public DiscoverCardActivateData(Vector2 sizeDelta, IDiscoverable discoverable)
        {
            _sizeDelta = sizeDelta;
            _discoverable = discoverable;
        }

        public float CardHeight => _sizeDelta.y;
        public float CardWidth => _sizeDelta.x;
        public CardViewData CardViewData => _discoverable.ViewData;
        public ReadOnlyRectTransform ReadOnlyRectTransform => _discoverable.RORTransform;
    }
}